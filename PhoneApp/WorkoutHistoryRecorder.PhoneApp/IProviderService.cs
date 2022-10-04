using System;
using System.Collections.Generic;
using System.Text;

namespace App2
{
    public interface IProviderService
    {
        bool CloseConnection();
        void SendData(string msg);

    }
}
