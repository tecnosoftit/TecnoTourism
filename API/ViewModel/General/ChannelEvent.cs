using System;
using Newtonsoft.Json;
using Exception = System.Exception;

namespace ViewModel.General
{
    public class ChannelEvent
    {
        public string Name { get; set; }

        public string ChannelName { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public object Data
        {
            get { return _data; }
            set
            {
                try
                {
                    _data = value;
                    this.Json = JsonConvert.SerializeObject(_data);
                }
                catch (Exception e)
                {
                    //
                }

            }
        }

        private object _data;

        public string Json { get; private set; }

        public ChannelEvent()
        {
            Timestamp = DateTimeOffset.Now;
        }
    }
}
