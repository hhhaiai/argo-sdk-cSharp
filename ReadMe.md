# C# SDK 使用说明

C# SDK 主要用于服务端 C# 应用，如 C# Web 应用的后台服务。

## 1. 集成准备

### 1.1 集成 SDK

####  动态链接库Analysys.dll的导入

以 Visual Studio 为例：将需要的动态链接库Analysys.dll拷贝到本地工程目录下；在 vs 中右键工程引用->添加引用->程序集->浏览，选择Analysys.dll点击 添加，即添加引用成功。`说明`：该版本 动态链接库基于 .netframework4.0 进行编译的，无需其他的第三方库，如果需要其他framework的版本，请联系我们。

## 2. SDK 初始化

### 2.1 获取配置信息

需获取的配置信息为：

* 数据接收地址：`http://host:port`

### 2.2 初始化接口

在控制台程序启动时program.cs（如 static void Main(string[] args) 方法中），调用构造函数 new AnalysysDotNetSdk(Collecter) 初始化 C# SDK 实例。如下：

``` cs
private const  string APP_KEY = "APPKEY";
private const  string ANALYSYS_SERVICE_URL = "http://host:port";
AnalysysDotNetSdk analysys = new AnalysysDotNetSdk(new SyncCollecter(ANALYSYS_SERVICE_URL), APP_KEY);
```

APPKEY：网站获取的 Key

SyncCollecter：为实时事件收集器

事件收集器还提供批量收集器，用户可以指定为实时收集和批量收集

#### 2.2.1 实时收集器

用户每触发一次上传，该收集器则立即上传数据至接收服务器：

``` cs
public SyncCollecter(string serverUrl);
```

* serverUrl：数据接收地址

#### 2.2.2 批量收集器

用户触发上传，该收集器将先缓存数据，直到数量达到用户设置的阈值或者用户设置的等待时间，才会触发真正的上传：

``` cs
public BatchCollecter(string serverUrl);
public BatchCollecter(string serverUrl, int batchNum);
public BatchCollecter(string serverUrl, int batchNum, long batchSec);
```

* serverUrl：数据接收地址
* batchNum：批量发送数量，默认值：20条
* batchSec：批量发送等待时间(秒)，默认值：10秒

至此 SDK初始化完成，调用 SDK 提供的接口已可正常采集用户数据了。需要注意：在多线程环境中时，推荐使用单例模式初始化 AnalysysDotNetSdk，开发者可以在不同的线程间重复使用 AnalysysDotNetSdk 实例用于记录数据。  
说明：程序结束前需要调用 `flush()` 接口，该接口可以把本地缓存的尚未发送至数据接收服务器的数据发送到数据接收服务器。

## 3. 基础接口介绍

### 3.1 Debug 模式

Debug 主要用于开发者测试，接口如下：

``` cs
public void SetDebugMode(DEBUG debug);
```

* debug：debug 模式,枚举类型,默认关闭状态。有以下三种枚举值：

  * `DEBUG.CLOSE`：表示关闭 Debug 模式
  * `DEBUG.OPENNOSAVE`：表示打开 Debug 模式，但该模式下发送的数据仅用于调试，不计入平台数据统计
  * `DEBUG.OPENANDSAVE`：表示打开 Debug 模式，该模式下发送的数据可计入平台数据统计

    <font color=red>注意：发布版本时debug模式设置为`DEBUG.CLOSE`。</font>

### 3.2 统计事件

事件跟踪，设置事件名称和事件详细信息。接口如下：

``` cs
public void Track(string distinctId, bool isLogin, string eventName, Dictionary<string, object> properties, string platform);
```

* distinctId：用户 ID,长度大于 0 且小于 255字符
* isLogin：用户 ID 是否是登录 ID
* eventName：事件名称,以字母或 `$` 开头，可包含字母、数字、下划线和 `$`，字母不区分大小写，`$`开头为预置事件,不支持乱码和中文,最大长度 99字符
* properties: 事件属性,最多包含 100条,且 key 以字母或 `$` 开头，可包含字母、数字、下划线和 `$`，字母不区分大小写，`$` 开头为预置事件属性,最大长度 125字符,不支持乱码和中文,value 类型约束(String/Number/bool/list/数组)，若为字符串,最大长度255字符
* platform：平台类型,内容范围：JS、WeChat、Android、iOS

示例：用户浏览商品

``` cs
// 浏览商品
Dictionary<string, object> trackPropertie = new Dictionary<string, object>();
trackPropertie.Add("$ip", "112.112.112.112"); //IP地址
List<string> bookList = new List<string>();
bookList.Add("Thinking in .net");
trackPropertie.Add("productName", bookList);  //商品列表
trackPropertie.Add("productType", ".net书籍");//商品类别
trackPropertie.Add("producePrice", 80);       //商品价格
trackPropertie.Add("shop", "xx网上书城");     //店铺名称
analysys.Track(distinctId, false, "ViewProduct", trackPropertie, platForm);
```

### 3.3 用户关联

用户 ID 关联接口。将 aliasId 和 distinctId 关联，计算时会认为是一个用户的行为。该接口是在 distinctId 发生变化的时候调用，来告诉 SDK distinctId 变化前后的 ID 对应关系。该场景一般应用在用户注册/登录的过程中。比如：一个匿名用户浏览商品，系统为其分配的distinctId = "1234567890987654321"，随后该匿名用户进行注册，系统为其分配了新的注册 ID，aliasId = "ABCDEF123456789"，此时就需要调用 alias 接口对两个 ID 进行关联。接口如下：

``` cs
public void Alias(string aliasId, string distinctId, string platform)
```

* aliasId：用户注册 ID，长度大于 0，且小于 255字符
* distinctId：用户匿名ID，长度大于 0，且小于 255字符
* platform：平台类型,内容范围：JS、WeChat、Android、iOS

示例：匿名用户浏览商品到注册会员

``` cs
// 匿名ID
string distinctId = "1234567890987654321";
string platform = "Android";
...
...
...
//用户注册登录
string registerId = "ABCDEF123456789";
analysys.Alias(registerId, distinctId, platForm);
```

### 3.4 用户属性设置

SDK提供以下接口供用户设置用户的属性，比如用户的年龄/性别等信息。

>用户属性是一个标准的K-V结构，K和V均有相应的约束条件，如不符合则丢弃该次操作。
参数约束:

* <h6 id="1.1">属性名称</h6>

    以字母或`$`开头，可包含字母、数字、下划线和`$`，字母不区分大小写，`$`开头为预置事件属性,最大长度125字符,不支持乱码和中文

* <h6 id="2">属性值</h6>

    支持部分类型：string/number/bool/集合/数组;
    若为字符串,则最大长度255字符;
    若为数组或集合,则最多包含100条,且key约束条件与属性名称一致,value最大长度255字符

设置单个或多个属性，如用户所在城市，用户昵称，用户头像信息等。如果之前存在，则覆盖，否则，新创建。接口如下：

``` cs
 public void ProfileSet(string distinctId, bool isLogin, Dictionary<string, object> properties, string platform);
```

* distinctId: 用户ID,长度大于0且小于255字符
* isLogin: 用户ID是否是登录 ID
* properties: 事件属性
* platform: 平台类型,内容范围：JS、WeChat、Android、iOS

示例：用户注册后设置用户的注册信息属性

``` cs
string registerId = "ABCDEF123456789";
bool isLogin = true;
string platform = "Android";
//用户信息
Dictionary<string, object> profiles = new Dictionary<string, object>();
profiles.Add("$city", "北京");      //城市
profiles.Add("$province", "北京");  //省份
profiles.Add("nickName", "昵称123");//昵称
profiles.Add("userLevel", 0);       //用户级别
profiles.Add("userPoint", 0);       //用户积分
List<string> interestList = new List<string>();
interestList.Add("户外活动");
interestList.Add("足球赛事");
interestList.Add("游戏");
profiles.Add("interest", interestList);//用户兴趣爱好
analysys.profileSet(registerId, isLogin, profiles, platform);
```

## 4. 更多接口

### 4.1 用户属性

#### 4.1.1 设置用户固有属性

只在首次设置时有效的属性。如：用户的注册时间。如果被设置的用户属性已存在，则这条记录会被忽略而不会覆盖已有数据，如果属性不存在则会自动创建。接口如下：

``` cs
public void ProfileSetOnce(string distinctId, bool isLogin, Dictionary<string, object> properties, string platform);
```

* distinctId: 用户ID,长度大于0且小于255字符
* isLogin: 用户ID是否是登录 ID
* properties: 事件属性
* platform: 平台类型,内容范围：JS、WeChat、Android、iOS

示例：要统计用户注册时间

``` cs
string registerId = "ABCDEF123456789";
bool isLogin = true;
string platform = "Android";
Dictionary<string, object> profile_age = new Dictionary<string, object>();
profile_age.Add("registerTime", "20180101101010");
analysys.profileSetOnce(registerId, isLogin, profile_age, platform);
```

#### 4.1.2 设置用户属性相对变化值

设置用户属性的单个相对变化值(相对增加,减少)，只能对数值型属性进行操作，如果这个Profile之前不存在,则初始值为0。接口如下：

``` cs
public void ProfileIncrement(string distinctId, bool isLogin, Dictionary<string, object> properties, string platform);
```

* distinctId: 用户ID,长度大于0且小于255字符
* isLogin: 用户ID是否是登录 ID
* properties: 事件属性
* platform: 平台类型,内容范围：JS、WeChat、Android、iOS

示例：用户注册初始积分为0，在用户购买商品后，用户的积分增加20，则调用该接口，用户的积分变为0+20=20了：

``` cs
string registerId = "ABCDEF123456789";
bool isLogin = true;
string platform = "Android";
Dictionary<string, object> profile = new Dictionary<string, object>();
profile.Add("userPoint",20);
analysys.ProfileIncrement(registerId, isLogin, profile, platform);
```

#### 4.1.3 增加列表类型的属性

为列表类型的属性增加一个或多个元素，如：用户新增兴趣爱好，接口如下：

``` cs
public void ProfileAppend(string distinctId, bool isLogin, Dictionary<string, object> properties, string platform);
```

* distinctId: 用户ID,长度大于0且小于255字符
* isLogin: 用户ID是否是登录 ID
* properties: 事件属性
* platform: 平台类型,内容范围：JS、WeChat、Android、iOS

示例：用户初始填写的兴趣爱好为["户外活动"，"足球赛事"，"游戏"]，调用该接口追加["学习"，"健身"]，则用户的爱好变为["户外活动"，"足球赛事"，"游戏"，"学习"，"健身"]

``` cs
string registerId = "ABCDEF123456789";
bool isLogin = true;
string platform = "Android";
Dictionary<string, object> profile = new Dictionary<string, object>();
List<string> interestList = new List<string>();
interestList.Add("户外活动");
interestList.Add("足球赛事");
interestList.Add("游戏");
profile.Add("interest", interestList);//用户兴趣爱好
analysys.ProfileAppend(registerId, isLogin, profile, platform);
```

#### 4.1.4 删除设置的属性

删除单个或所有已设置的属性。接口如下：

``` cs
public void ProfileUnSet(string distinctId, bool isLogin, string property, string platform);
public void ProfileDelete(string distinctId, bool isLogin, string platform);
```

* distinctId: 用户ID,长度大于0且小于255字符
* isLogin: 用户ID是否是登录 ID
* propertie: 事件属性
* platform: 平台类型,内容范围：JS、WeChat、Android、iOS

示例1： 要删除已经设置的用户昵称这一用户属性

``` cs
string registerId = "ABCDEF123456789";
bool isLogin = true;
string platform = "Android";
// 删除单个用户属性
analysys.ProfileUnSet(registerId, isLogin, "nickName", platform);
```

示例2：要清除已经设置的所有用户属性

``` cs
string registerId = "ABCDEF123456789";
bool isLogin = true;
string platform = "Android";
// 清除所有属性
analysys.ProfileDelete(registerId, isLogin, platform);
```

### 4.2 通用属性

如果某个事件的属性，在所有事件中都会出现，则这个属性可以做为通用属性，通过 RegisterSuperProperties() 将该属性设置为事件通用属性，则设置后每次发送事件的时候都会带有该属性。比如用户浏览/购买商品过程中的用户等级就可以作为通用属性。

>通用属性是一个标准的 K-V 结构，K 和 V 均有相应的约束条件，如不符合则丢弃该次操作。

约束条件如下:

* <h6 id="1">属性名称</h6>

    以字母或 `$` 开头，可包含字母、数字、下划线和 `$`，字母不区分大小写，`$` 开头为预置事件属性,最大长度 125字符,不支持乱码和中文

* <h6 id="2">属性值</h6>

  * 支持部分类型：string/number/bool/集合/数组;
  * 若为字符串,则最大长度 255字符;
  * 若为数组或集合,则最多包含 100条,且 key 约束条件与属性名称一致,value 最大长度 255字符

#### 4.2.1 注册通用属性

以用户浏览/购买商品的过程中发生的事件为例，用户的级别/积分就可以作为通用的属性，通过把用户级别/积分注册为通用属性，就可以避免在每次收集事件属性的时候都要手工设置该属性值。接口如下：

``` cs
public void RegisterSuperProperties(Dictionary<string, object> superProperties);
```

* superProperties：设置多个属性

示例：

``` cs
// 设置多个通用属性
Dictionary<string, Object>() superPropertie = new Dictionary<string, Object>();
superPropertie.Add("userLevel", 0); //用户级别
superPropertie.Add("userPoint", 0); //用户积分
analysys.RegisterSuperProperties(superPropertie);
```

#### 4.2.2 删除通用属性

如果要删除某个通用属性或者删除全部的通用属性，可以分别调用 UnregisterSuperProperty 或 ClearSuperProperties 接口。具体接口定义如下：

``` cs
public void UnRegisterSuperProperty(string key);
public void ClearSuperProperties();
```

* key：属性名称

示例1：删除设置的用户积分属性

``` cs
// 删除单个通用属性
analysys.UnRegisterSuperProperty("userPoint");
```

示例2：清除所有已经设置的通用属性

``` cs
// 清除所有通用属性
analysys.ClearSuperProperties();
```

#### 4.2.3 获取通用属性

由属性名称查询获取单条通用属性，或者获取全部的通用属性。接口如下：

``` cs
public object GetSuperProperty(string key);
public Dictionary<string, object> GetSuperProperties();
```

* key：属性名称

示例1：查看已经设置的 userLevel 通用属性

``` cs
// 获取单个通用属性
analysys.GetSuperProperty("userLevel");
```

示例2：查看所有已经设置的通用属性

``` cs
// 获取所有通用属性
analysys.GetSuperProperties();
```

### 4.3  刷新缓存

立即发送所有收集的信息到服务器。

``` cs
analysys.Flush();
```

## 5. SDK 使用样例

``` cs
private const  string APP_KEY = "1234";
private const  string ANALYSYS_SERVICE_URL = @"http://192.168.220.167:8089";
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
    trackPropertie.Add("productType", ".net书籍");//商品类别
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
```
