using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Utilities
{
    public static class ErrorResources
    {
        public static string NotExistWithId(string tableName, string Id)
        {
            return $"There is not an instance of {tableName} with {tableName}Id = {Id}!";
        }
    }
}
