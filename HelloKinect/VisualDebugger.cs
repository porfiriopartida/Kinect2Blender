using System;
using System.Windows.Forms;
public class VisualDebugger 
{
    public TextBox txtDebugger { get; set; }
    public VisualDebugger()
    {
        this.txtDebugger = txtDebugger;
    }
    public void Write(string text)
    {
        if (this.txtDebugger != null)
        {
            AppendText(text);
        }
        else {
            Console.Write(text);
        }
    }
    public void WriteLine(string text)
    {
        this.Write(text + "\n\r");
    }
    delegate void SetTextCallback(string text);
    private void AppendText(string text)
    {
        Console.Write(text);
        // InvokeRequired required compares the thread ID of the
        // calling thread to the thread ID of the creating thread.
        // If these threads are different, it returns true.
        if (txtDebugger.InvokeRequired)
        {
            SetTextCallback d = new SetTextCallback(AppendText);
            txtDebugger.Invoke(d, new object[] { text });
        }
        else
        {
            txtDebugger.AppendText(text);
        }
    }
}
