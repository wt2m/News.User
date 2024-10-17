using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.Interfaces;

namespace UserService.Application.Services
{
    public abstract class AbstractService
    {
        public IUnitOfWork _unitOfWork;
        public AbstractService(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }
    }
}
