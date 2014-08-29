// PInvokeTest.cs
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;

class PlatformInvokeTest
{
    [DllImport("msvcrt.dll")]
    public static extern int puts(string c);
    [DllImport("msvcrt.dll")]
    internal static extern int _flushall();


    [DllImport("user32.dll", EntryPoint = "GetWindowText",
  CharSet = CharSet.Auto)]
    static extern IntPtr GetWindowCaption(IntPtr hwnd,
      StringBuilder lpString, int maxCount);

    static string GetWindowCaption(IntPtr hwnd)
    {
        StringBuilder sb = new StringBuilder(256);
        GetWindowCaption(hwnd, sb, 256);
        return sb.ToString();
    }

    [DllImport("user32.dll", EntryPoint = "SendMessage",
  CharSet = CharSet.Auto)]
    static extern int SendMessage3(IntPtr hwndControl, uint Msg,
      int wParam, StringBuilder strBuffer); // get text

    [DllImport("user32.dll", EntryPoint = "SendMessage",
      CharSet = CharSet.Auto)]
    static extern int SendMessage4(IntPtr hwndControl, uint Msg,
      int wParam, int lParam);  // text length

    [DllImport("user32.dll", EntryPoint = "FindWindowEx",
  CharSet = CharSet.Auto)]
    static extern IntPtr FindWindowEx(IntPtr hwndParent,
      IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

    [DllImport("user32.dll", EntryPoint = "FindWindow")]
    private static extern IntPtr FindWindow(string lp1, string lp2);

    [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetForegroundWindow(IntPtr hWnd);



    static int GetTextBoxTextLength(IntPtr hTextBox)
    {
        // helper for GetTextBoxText
        uint WM_GETTEXTLENGTH = 0x000E;
        int result = SendMessage4(hTextBox, WM_GETTEXTLENGTH,
          0, 0);
        return result;
    }

    static string GetTextBoxText(IntPtr hTextBox)
    {
        uint WM_GETTEXT = 0x000D;
        int len = GetTextBoxTextLength(hTextBox);
        if (len <= 0) return null;  // no text
        StringBuilder sb = new StringBuilder(len + 1);
        SendMessage3(hTextBox, WM_GETTEXT, len + 1, sb);
        return sb.ToString();
    }

    static List<IntPtr> GetAllChildrenWindowHandles(IntPtr hParent,
  int maxCount)
    {
        List<IntPtr> result = new List<IntPtr>();
        int ct = 0;
        IntPtr prevChild = IntPtr.Zero;
        IntPtr currChild = IntPtr.Zero;
        while (true && ct < maxCount)
        {
            currChild = FindWindowEx(hParent, prevChild, null, null);
            if (currChild == IntPtr.Zero) break;
            result.Add(currChild);
            prevChild = currChild;
            ++ct;
        }
        return result;
    }

    public static void Main()
    {
        puts("Test");
        _flushall();

        // launch app first
        IntPtr appHandle = FindWindow(null, "Form1"); // P/Invoke
        Console.WriteLine("App handle = " + appHandle.ToString("X"));
        List<IntPtr> children = GetAllChildrenWindowHandles(appHandle, 100);

        Console.WriteLine("Children handles are:");
        for (int i = 0; i < children.Count; ++i)
            Console.WriteLine(children[i].ToString("X"));

        // get handle to app
        // get and store handles to children
        Console.WriteLine("The text/caption of each child" +
          "control window is:");
        for (int i = 0; i < children.Count; ++i)
        {
            IntPtr hControl = children[i];
            string caption = GetTextBoxText(hControl);
            Console.WriteLine(caption);
        }

    }
}