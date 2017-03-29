/* A simple multi-threaded Port Scanner
 * Author: Munir Usman http://munir.wordpress.com
 * Contact: munirus@gmail.com 
 */

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace PortScanner
{
    class Program
    {
        static void Main(string[] args)
        {
            string host;
            int portStart = 1, portStop = 65535, ctrThread = 200;

            try
            {
                Console.WriteLine("Taranacak Ýp Adresi:");
                host = Console.ReadLine();
            }
            catch
            {
                printUsage();
                return;
            }

            PortScanner ps = new PortScanner(host, portStart, portStop);
            ps.start(ctrThread);
        }

        static void printUsage()
        {
            Console.WriteLine("0 - 65535 tüm portlarý tarar.\n");
        }

    }

    public class PortScanner
    {
        private string host;
        private PortList portList;

        public PortScanner(string host, int portStart, int portStop)
        {
            this.host = host;
            this.portList = new PortList(portStart, portStop);
        }

        public PortScanner(string host)
            : this(host, 1, 65535)
        {
        }

        public PortScanner()
            : this("127.0.0.1")
        {
        }

        public void start(int threadCtr)
        {
            for (int i = 0; i < threadCtr; i++)
            {
                Thread th = new Thread(new ThreadStart(run));
                th.Start();
            }
        }
        public void run()
        {
            int port;
            TcpClient tcp = new TcpClient();
            while ((port = portList.getNext()) != -1)
            {
                try
                {
                    tcp = new TcpClient(host, port);
                }
                catch
                {
                    continue;
                }
                finally
                {
                    try
                    {
                        tcp.Close();
                    }
                    catch { }
                }
                Console.WriteLine("TCP Port " + port + " açýk.");
            }
        }
    }
    public class PortList
    {
        private int start;
        private int stop;
        private int ptr;

        public PortList(int start, int stop)
        {
            this.start = start;
            this.stop = stop;
            this.ptr = start;
        }
        public PortList() : this(1, 65535)
        {
        }

        public bool hasMore()
        {
            return (stop - ptr) >= 0;
        }
        public int getNext()
        {
            if (hasMore())
                return ptr++;
            return -1;
        }
    }
}
