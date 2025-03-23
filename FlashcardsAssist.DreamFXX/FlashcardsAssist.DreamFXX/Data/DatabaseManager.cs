using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashcardsAssist.DreamFXX.Data;
public class DatabaseManager
{
    private readonly string _connectionString;
    public DatabaseManager(string connectionString)
    {
        _connectionString = connectionString;
    }


}
