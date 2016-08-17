using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondHandStoreApp.Interfaces
{
    interface IGenericInterface<T>
    {
        List<T> GetAll();
        T GetById(int id);

        bool Create(T obj);
        bool Update(T obj);
        bool Delete(int id);
    }
}
