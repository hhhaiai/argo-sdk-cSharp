using Analysys;
using System;
using System.Collections.Generic;

namespace Demo
{
    class Program
    {
        private const  string APP_KEY = "1234";
        private const  string ANALYSYS_SERVICE_URL = @"http://192.168.220.167:8089";
        static void Main(string[] args)
        {
            AnalysysDotNetSdk analysys = new AnalysysDotNetSdk(new SyncCollecter(ANALYSYS_SERVICE_URL), APP_KEY);
            try
            {
                string distinctId = "1234567890987654321";
                string platForm = "DotNet"; //DotNet平台
                analysys.SetDebugMode(DEBUG.OPENNOSAVE); //设置debug模式
                //浏览商品
                Dictionary<string, object> trackPropertie = new Dictionary<string, object>();
                trackPropertie.Add("$ip", "112.112.112.112"); //IP地址
                List<string> bookList = new List<string>();
                bookList.Add("Thinking in .net");
                trackPropertie.Add("productName", bookList);  //商品列表
                trackPropertie.Add("productType", "Java书籍");//商品类别
                trackPropertie.Add("producePrice", 80);       //商品价格
                trackPropertie.Add("shop", "xx网上书城");     //店铺名称
                analysys.Track(distinctId, false, "ViewProduct", trackPropertie, platForm);

                //用户注册登录
                string registerId = "ABCDEF123456789";
                analysys.Alias(registerId, distinctId, platForm);

                //设置公共属性
                Dictionary<string, object> superPropertie = new Dictionary<string, object>();
                superPropertie.Add("sex", "male"); //性别
                superPropertie.Add("age", 23);     //年龄
                analysys.RegisterSuperProperties(superPropertie);
                //用户信息
                Dictionary<string, object> profiles = new Dictionary<string, object>();
                profiles.Add("$city", "北京");        //城市
                profiles.Add("$province", "北京");  //省份
                profiles.Add("nickName", "昵称123");//昵称
                profiles.Add("userLevel", 0);       //用户级别
                profiles.Add("userPoint", 0);       //用户积分
                List<string> interestList = new List<string>();
                interestList.Add("户外活动");
                interestList.Add("足球赛事");
                interestList.Add("游戏");
                profiles.Add("interest", interestList);//用户兴趣爱好
                analysys.ProfileSet(registerId, true, profiles, platForm);

                //用户注册时间
                Dictionary<string, object> profile_age = new Dictionary<string, object>();
                profile_age.Add("registerTime", "20180101101010");
                analysys.ProfileSetOnce(registerId, true, profile_age, platForm);

                //重新设置公共属性
                analysys.ClearSuperProperties();
                superPropertie.Clear();
                superPropertie = new Dictionary<string, Object>();
                superPropertie.Add("userLevel", 0); //用户级别
                superPropertie.Add("userPoint", 0); //用户积分
                analysys.RegisterSuperProperties(superPropertie);

                //再次浏览商品
                trackPropertie.Clear();
                trackPropertie.Add("$ip", "112.112.112.112"); //IP地址
                List<string> abookList = new List<string>();
                abookList.Add("Thinking in Java");
                trackPropertie.Add("productName", bookList);  //商品列表
                trackPropertie.Add("productType", "Java书籍");//商品类别
                trackPropertie.Add("producePrice", 80);       //商品价格
                trackPropertie.Add("shop", "xx网上书城");     //店铺名称
                analysys.Track(registerId, true, "ViewProduct", trackPropertie, platForm);

                //订单信息
                trackPropertie.Clear();
                trackPropertie.Add("orderId", "ORDER_12345");
                trackPropertie.Add("price", 80);
                analysys.Track(registerId, true, "Order", trackPropertie, platForm);

                //支付信息
                trackPropertie.Clear();
                trackPropertie.Add("orderId", "ORDER_12345");
                trackPropertie.Add("productName", "Thinking in Java");
                trackPropertie.Add("productType", "Java书籍");
                trackPropertie.Add("producePrice", 80);
                trackPropertie.Add("shop", "xx网上书城");
                trackPropertie.Add("productNumber", 1);
                trackPropertie.Add("price", 80);
                trackPropertie.Add("paymentMethod", "AliPay");
                analysys.Track(registerId, true, "Payment", trackPropertie, platForm);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                analysys.Flush();
            }

            Console.ReadKey();

        }
    }
}
