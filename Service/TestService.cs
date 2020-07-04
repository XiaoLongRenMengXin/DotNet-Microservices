using System;
using Dao;
using Mapper;
namespace Service
{
    public class TestService
    {
        public static string Showing() {
            return TestDao.Showing();
        }

        public static string Show() {
            return TestMapper.Showing();
        }
    }
}
