using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBoardsMinimalAPI.Dto
{
    public class EpicDto
    {
        public int Id { get; set; }
        public int Priority { get; set; }
        public string Area { get; set; }
        public DateTime? StartDate { get; set; }
        public string AuthorFullName { get; set; }
    }
}
