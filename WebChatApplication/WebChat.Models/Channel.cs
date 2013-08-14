using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebChat.Models
{
    public class Channel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int FirstUserId { get; set; }
        public virtual User FirstUser { get; set; }

        public int SecondUserId { get; set; }
        public virtual User SecondUser { get; set; }

    }
}
