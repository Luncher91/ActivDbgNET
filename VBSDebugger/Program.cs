using ActivDbg;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace VBSDebugger
{
    class Program
    {
        /* To test this application start cscript with /x
         * cscript /x helloWorld.vbs
         * /
        
        /* Here is some documentation on making a system ready to debug scripts with this kind of tools:
         * 
         * http://digital.ni.com/public.nsf/allkb/E6AB36F78F24CAFE8625809700546004
         * or RegisterDebugger.oxps
         * 
         * Windows 7/8/8.1 and DIAdem 64-bit:
         * Regsvr32.exe "C:\Program Files\Internet Explorer\pdm.dll"
         * 
         * Windows 7/8/8.1 and DIAdem 32-bit:
         * Regsvr32.exe "C:\Program Files (x86)\Internet Explorer\pdm.dll"
         * 
         * Windows 10 (up to version 1511) and DIAdem 64-bit:
         * Regsvr32.exe "C:\Windows\System32\F12\pdm.dll"
         * 
         * Windows 10 (up to version 1511) and DIAdem 32-bit:
         * Regsvr32.exe "C:\Windows\SysWOW64\F12\pdm.dll"
         * 
         */

        /* LIST AS JIT
         * Another thing to note is, that this tool is not listed among the debuggers 
         * if the JIT Debugger selection pops up: find out how to list an application there!
         * 
         * VSCODE EXTENSION
         * The final VSCode extension will have some script which has to be executed 
         * as an Administrator once, which does all the necessary things as far as possible.
         */

        /* We can stop and resume on a break point
         * we can list all loaded documents
         * we can show the content of the documents
         * we have an attribte for each character of the document
         * 
         * 
         */

        /* I would like to see the position of the next execution step
         * I would like to see exploring objects
         * I would like to see executing inline code
         * I would like to single step, step over and step in, step out
         * 
         * I would like to know how I can add a launcher to the MDM (see: LIST AS JIT)
         */

        /* When all that is fullfilled I can start to connect this Library with an VSCode Extension!
         * (see: VSCODE EXTENSION)
         * 
         */

        static DebugProcess cscriptProc;
        static bool Connected = true;
        static Stack<DebugApplication> breakPoints = new Stack<DebugApplication>();

        [STAThread]
        static void Main(string[] args)
        {
            cscriptProc = ConsoleSelectDebugProcess();

            if (cscriptProc == null)
                return;

            var ad = new ApplicationDebugger();

            Console.WriteLine("Connecting...");
            cscriptProc.ConnectDebugger(ad);
            Console.WriteLine("Connected to " + cscriptProc.Name);
            // TODO: make the breakpoint thing more event based;
            //       so extend ApplicationDebugger by some events and register an event which just resumes on a key press

            ad.BreakPoint += Handle_BreakPoint;

            ad.Close += Handle_Close;

            Console.WriteLine("Waiting for BreakPoint...");

            while (Connected)
            {
                while(breakPoints.Any())
                {
                    var bp = breakPoints.Pop();
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadKey();
                    cscriptProc.Resume(bp);
                    Console.WriteLine("Resumed!");
                }

                Thread.Sleep(300);
            }
        }

        public static DebugProcess ConsoleSelectDebugProcess()
        {
            VBSDebugger vbsD = new VBSDebugger();
            var processes = vbsD.GetProcesses();
            DebugProcess proc = null;

            while (proc == null)
            {
                Console.Clear();
                Console.WriteLine("Please select a process to debug...");
                ListProcesses(processes);
                Console.WriteLine("X: CACNEL");
                Console.WriteLine("R: REFRESH");
                string selection = Console.ReadLine().ToUpperInvariant();
                if(selection == "X")
                {
                    break;
                }
                else if(selection == "R")
                {
                    continue;
                }
                else
                {
                    int selectedProcess = -1;
                    bool success = int.TryParse(selection, out selectedProcess);
                    if (success && selectedProcess >= 0 && selectedProcess < processes.Length)
                        proc = processes[selectedProcess];
                }
            }

            return proc;
        }

        public static void ListProcesses(DebugProcess[] procs)
        {
            for (int i = 0; i < procs.Length; i++)
            {
                var proc = procs[i];
                Console.WriteLine(i + ": " + proc.Name);
            }
        }

        private static void Handle_Close()
        {
            Connected = false;
        }

        private static void Handle_BreakPoint(DebugApplication app, BreakReason reason, ScriptError error)
        {
            Console.WriteLine("BreakReason: " + reason.ToString());
            breakPoints.Push(app);

            var docs = app.GetDocuments();
            var errorDoc = error?.GetTextDocument();
            var errorPosition = new ScriptError.DocumentPosition() { Line = 0, Character = 0 };
            error?.GetDebugPosition();

            foreach (var doc in docs)
            {
                // Attributes can be used to color the text
                var docText = doc.GetText();

                Console.WriteLine("Title: " + doc.GetName());
                Console.WriteLine("CONTENT START");
                var textLines = Regex.Split(docText.Text, "\r\n|\r|\n");
                for (int i = 0; i < textLines.Length; i++)
                {
                    string l = textLines[i];

                    if (doc.GetName() == errorDoc?.GetName() && i == errorPosition.Line)
                    {
                        l = "»" + l + "«";
                    }

                    Console.WriteLine(l);
                }
                Console.WriteLine("EOF");
            }

            Console.WriteLine("Added to the BreakPoint-Stack!");
        }
    }
}
