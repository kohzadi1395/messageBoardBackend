using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MessageBoardBackend.Models
{
    [Table("Message")]
    public class Message
    {
        private Guid _id;

        [Key]
        public Guid Id
        {
            get
            {
                if (_id == Guid.Empty)
                    _id = Guid.NewGuid();
                return _id;
            }
            set => _id = value;
        }

        public string Owner { get; set; }
        public string Text { get; set; }
    }
}