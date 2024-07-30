using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanB.DPF.Manager.Api.Contracts.DataTransferObjects
{
    public class Mp3Dto
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }

        public Mp3Dto()
        {

        }

        public Mp3Dto(string fileName, string contentType, byte[] content)
        {
            this.FileName = fileName;
            this.ContentType = contentType;
            this.Content = content;
        }
    }

}
