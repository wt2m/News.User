using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Domain.Entities.Abstract
{
    public abstract class PersistentData
    {
        public Guid Id { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public bool Deleted { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        public PersistentData() {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
            Deleted = false;
        }

        public void ChangeToDeleted()
        {
            if (Deleted)
                throw new Exception("Entity already deleted.");

            Deleted = true;
            DeletedAt = DateTime.Now;
        }

        public void RovokeDeletion()
        {
            if (!Deleted)
                return;

            Deleted = false;
            DeletedAt = null;
        }
    }
}
