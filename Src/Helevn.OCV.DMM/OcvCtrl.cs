using System.Net.Sockets;
using System.Text;

namespace Helevn.OCV.DMM
{
    public class OcvCtrl(string name)
    {
        /// <summary>
        /// 名称
        /// </summary>
        public readonly string Name = name;
        private TcpClient? _tcpClient = null;
        public bool Connect => _tcpClient?.Connected ?? false;

        /// <summary>
        /// 小数点保留位数
        /// </summary>
        public int Digits { get; set; } = 5;
        /// <summary>
        /// 最大值限位
        /// </summary>
        public int LimitMaxValue { get; set; } = 99999;
        /// <summary>
        /// 最小值限位
        /// </summary>
        public int LimitMinValue { get; set; } = -99999;
        /// <summary>
        /// 读取超时限制
        /// <para/>
        /// ms
        /// </summary>
        public int LimitOverTime { get; set; } = 1000;

        /// <summary>
        /// IP地址
        /// </summary>
        public string IpAddress { get; set; } = "127.0.0.1";
        /// <summary>
        /// IP端口号
        /// </summary>
        public int IpPort { get; set; } = 502;

        public async Task<double> ReadAsync(byte[] cmds)
        {
            try
            {
                var stream = _tcpClient!.GetStream();
                await stream.WriteAsync(cmds);
                byte[] rbuffer = new byte[256];
                var revleng = await stream.ReadAsync(rbuffer, new CancellationTokenSource(LimitOverTime).Token);
                byte[] recbuffer = new byte[revleng];
                for (int i = 0; i < revleng; i++)
                {
                    recbuffer[i] = rbuffer[i];
                }
                var str = Encoding.Default.GetString(recbuffer);
                var r = double.TryParse(str, out double v);
                if (!r)
                    v = double.NaN;
                v = Math.Round(v, Digits);
                v = Math.Min(v, LimitMaxValue);
                v = Math.Max(v, LimitMinValue);
                return v;
            }
            catch
            {
                throw;
            }
        }
        public async Task EnsureConnectAsync()
        {
            _tcpClient ??= new TcpClient();
            if (!_tcpClient.Connected)
            {
                await _tcpClient.ConnectAsync(IpAddress, IpPort);
            }
        }
        public Task DisposeAsync()
        {
            if (_tcpClient != null)
            {
                _tcpClient.Close();
                _tcpClient.Dispose();
                _tcpClient = null;
            }
            return Task.CompletedTask;
        }
    }
}
