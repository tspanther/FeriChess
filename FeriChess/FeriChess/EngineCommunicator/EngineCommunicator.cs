using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace FeriChess.EngineCommunicator
{
    public class EngineCommunicator: IEngineCommunicator
    {
        private Process StockFishProcess;
        private StreamWriter writer;
        private StreamReader reader;
        //20 milliseconds is the preset value
        private int MoveTime = 20;
        /// <summary>
        /// The constructor EngineCommunicator() creates a new process and attaches it to stockfish
        /// </summary>
        public EngineCommunicator()
        {
            StockFishProcess = new Process();
            //Path to the file
            StockFishProcess.StartInfo.FileName = @"C:\FeriChess\FeriChess\FeriChess\EngineCommunicator\stockfish_10_x64.exe";
            StockFishProcess.StartInfo.RedirectStandardInput = true;
            StockFishProcess.StartInfo.RedirectStandardOutput = true;
            StockFishProcess.StartInfo.UseShellExecute = false;
            StockFishProcess.Start();
            writer = StockFishProcess.StandardInput;
            reader = StockFishProcess.StandardOutput;
        }
        /// <summary>
        /// The destructor closes Stockfish with the exit keyword quit and than
        /// frees recources tied to the process
        /// </summary>
        ~EngineCommunicator()
        {
            //close Stockfish
            writer.WriteLine("quit");
            StockFishProcess.Close();
        }
        /// <summary>
        /// the function sets the number of threads that stockfish will use
        /// </summary>
        /// <param name="number">number of threads</param>
        public void SetThreads(int number)
        {
            writer.WriteLine("setoption name Threads value {0}", number);
        }
        /// <summary>
        /// the function sets the number of threads of milisecords stockfish will run at
        /// </summary>
        /// <param name="number">number of miliseconds</param>
        public void SetMoveTime(int number)
        {
            MoveTime = number;
        }
        /// <summary>
        /// this is a function to trim the result into the data we need
        /// </summary>
        /// <param name="output">the data to be processed </param>
        /// <returns>return the data we want</returns>
        private string TrimOutput(string output)
        {
            return output.Substring(9, 4);
        }
        /// <summary>
        /// This function returns the next best move
        /// </summary>
        /// <param name="FEN">standart FEN notation</param>
        /// <returns>best move </returns>
        public string NextMove(string FEN)
        {
            string output = String.Empty;
            try
            {
                reader.ReadLine();//get autor to remove it from standart output
                //input move
                writer.WriteLine("position fen {0}", FEN);
                //start search
                writer.WriteLine("go move time " + MoveTime);
                bool done = false;
                //get the result
                while (!done)
                {
                    output = reader.ReadLine();
                    if (output.StartsWith("bestmove"))
                    {
                        done = true;
                    }
                }
            }
            catch (Exception)
            {
                //Console.WriteLine(e.ToString());
            }
            return TrimOutput(output);
        }
    }
}