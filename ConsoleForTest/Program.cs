using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using MihaZupan;
using Redmine.Net.Api;
using Redmine.Net.Api.Async;
using Redmine.Net.Api.Types;
using RedmineBot.Services;
using Telegram.Bot;

namespace ConsoleForTest
{
    class Program
    {
        private static readonly HttpToSocks5Proxy Proxy = new HttpToSocks5Proxy("31.13.224.12", 9999);
        private const string BotToken = "631170979:AAFsyz1LJ17GeDGJ6MfSaEl6P0PJKfO81q0";
        //private const string RedmineApiKey = "c52fce8bc7508b51915f3a0b928d3f506ced9a65";//наиль
        private const string RedmineApiKey = "e502c3df9e31fb3a4b5b0b522d609a1bb444fa2e";//рустик
        private const string RedmineHost = "https://rd.d-l-s.ru/";

        //private const string login = "nshakirov";
        //private const string pass = "Z!1eZwVm";

        enum IssueStatus : byte
        {
            New = 1,
            InWork,
            Done,
            OnPause,
            Closed
        }

        static async Task Main(string[] args)
        {
            
            //userId - 65 - Саша
            var manager = new RedmineManager(RedmineHost, RedmineApiKey);
            //var manager = new RedmineManager(RedmineHost, login, pass);

            //parameter - fetch associated relations.
            //status 1 - new , 2 - в работе, 3 - решена, 4 - на паузе, 5 - закрыта
            //var parameters = new NameValueCollection {
            //    //{ RedmineKeys.STATUS_ID, $"{IssueStatus.InWork:D}|{IssueStatus.New:D}"},
            //    // { RedmineKeys.DUE_DATE, GetLastDay().ToString("yyyy-MM-dd")}, { RedmineKeys.START_DATE, "><2018-11-01|2018-11-22" },
            //    { RedmineKeys.ISSUE_ID, "26508" }
            //};
            //parameter - fetch issues for a date range
            //parameters.Add(RedmineKeys.CREATED_ON, "><2012-03-01|2012-03-07");

            //var user = await manager.GetCurrentUserAsync();

            //var attac = await manager.GetObjectsAsync<TimeEntry>(parameters);
            //{
            //    Console.WriteLine("Issue: {0}.", issue);
            //}
            //Proxy.ResolveHostnamesLocally = true;

            //TelegramBotClient Bot = new TelegramBotClient(BotToken, Proxy);
            //Bot.OnMessage += Bot_OnMessage;
            //var s = attac.Objects[1];
            //var iss = new Issue();

            //iss.AssignedTo = new IdentifiableName {Id = 107};
            //iss.Author = new IdentifiableName {Id = 107};
            //iss.CreatedOn = DateTime.Now;
            //iss.CustomFields = new List<IssueCustomField>
            //    {
            //        new IssueCustomField{Id = 6, Multiple = false/*, Name="Тип"*/, Values = new List<CustomFieldValue> { new CustomFieldValue { Info = "Тех" }}},
            //        new IssueCustomField{Id = 13, Multiple = false/*, Name="Deadline"*/, Values = new List<CustomFieldValue> {new CustomFieldValue{Info = "2018-10-31" } }}
            //    };
            //iss.Description = "";
            //iss.DoneRatio = 0;
            //iss.Priority = new IdentifiableName {Id = 2};//normal
            //iss.Project = new IdentifiableName {Id = 30};//tu dji host
            //iss.StartDate = DateTime.Today;
            //iss.Status = new IdentifiableName {Id = 1}; //new
            //iss.Subject = "Правка временной зоны react";
            //iss.Tracker = new IdentifiableName {Id = 5};
            //iss.UpdatedOn = DateTime.Now;
            //data выполнения
            //оценка времени


            //await manager.CreateObjectAsync(iss);

            //User currentUser = manager.GetCurrentUser();
            //Console.WriteLine($"Current user: {currentUser}.");
            //Bot.StartReceiving();
           

          
            //IRedmineService redmineService = new RedmineService(manager);
            //IUpdateService updateService = new UpdateService(redmineService);

            //try
            //{
            //    //await updateService.SpendTime();
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //}
            //Console.WriteLine("success");
            //Console.ReadLine();
            //Bot.StopReceiving();
        }

        private static void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
            {
               
            }
        }

        private static DateTime GetLastDay()
        {
            var today = DateTime.Today;
            var lastDay = DateTime.DaysInMonth(today.Year, today.Month);
            return new DateTime(today.Year, today.Month, lastDay);
        }

        private static Issue GetIssue()
        {
            return new Issue
            {
                AssignedTo= new IdentifiableName { Id= 107 },
                Author = new IdentifiableName { Id= 107 },
                CreatedOn = DateTime.Now,
                CustomFields =
                {
                    new IssueCustomField{Id = 6, Multiple = false/*, Name="Тип"*/, Values = { new CustomFieldValue { Info = "Тех" }}},
                    new IssueCustomField{Id = 13, Multiple = false/*, Name="Deadline"*/, Values = {new CustomFieldValue{Info = DateTime.Today.ToString(CultureInfo.InvariantCulture)}}}
                },
                Description = "",
                DoneRatio = 0,
                Priority = new IdentifiableName { Id= 2 },//normal
                Project = new IdentifiableName { Id = 30 },//tu dji host
                StartDate = DateTime.Today,
                Status = new IdentifiableName { Id = 1 }, //new
                Subject = "Правка временной зоны react",
                Tracker = new IdentifiableName { Id = 5 },
                UpdatedOn = DateTime.Now
            };
        }
    }
}
//example
//107 


// EstimatedHours = 30
//+		AssignedTo	{[IdentifiableName: Id=1, Name=Вадим Кляшев]}	Redmine.Net.Api.Types.IdentifiableName
//+		Author	{[IdentifiableName: Id=1, Name=Вадим Кляшев]}	Redmine.Net.Api.Types.IdentifiableName
//+		CreatedOn	{30.10.2018 9:29:40}	System.DateTime?
//-		CustomFields	Count = 2	System.Collections.Generic.IList<Redmine.Net.Api.Types.IssueCustomField> {System.Collections.Generic.List<Redmine.Net.Api.Types.IssueCustomField>}
//+		[0]	{[IssueCustomField: [IdentifiableName: Id=6, Name=Тип] Values=System.Collections.Generic.List`1[Redmine.Net.Api.Types.CustomFieldValue], Multiple=False]}	Redmine.Net.Api.Types.IssueCustomField
//      Values = Count = 1 [0] = {[CustomFieldValue: Info=Тех]}
//+		[1]	{[IssueCustomField: [IdentifiableName: Id=13, Name=Deadline] Values=System.Collections.Generic.List`1[Redmine.Net.Api.Types.CustomFieldValue], Multiple=False]}	Redmine.Net.Api.Types.IssueCustomField
//                          [0] = {[CustomFieldValue: Info=2018-10-31]}
//		Description	""	string
//		DoneRatio	0	float?
//		IsPrivate	false	bool
//+		Priority	{[IdentifiableName: Id=2, Name=Нормальный]}	Redmine.Net.Api.Types.IdentifiableName
//+		Project	{[IdentifiableName: Id=30, Name=09) DLS Smart Purchase 2G Хост (Вадим Кляшев)]}	Redmine.Net.Api.Types.IdentifiableName
//+		StartDate	{01.10.2018 0:00:00}	System.DateTime?
//+		Status	{[IdentifiableName: Id=2, Name=В работе]}	Redmine.Net.Api.Types.IdentifiableName
//		Subject	"Администрирование серверов и инфраструктуры"	string
//+		Tracker	{[IdentifiableName: Id=5, Name=Задача]}	Redmine.Net.Api.Types.IdentifiableName
//+		UpdatedOn	{30.10.2018 9:29:40}	System.DateTime?


//TimeEntry
//Activity = {[IdentifiableName: Id=9, Name=Разработка]}
//Comments = "оперативка по SwS, совещание с  Леонидом по вопросам ВТБ, обсуждение итогов тестирования ЛК ТСП "
//CreatedOn = {31.10.2018 16:42:51}
//Hours = 1.92
//Issue = {[IdentifiableName: Id=26287, Name=]}
//Project = {[IdentifiableName: Id=546, Name=00) SwS-продукт]}
//SpentOn = {31.10.2018 0:00:00}
//UpdatedOn = {31.10.2018 16:42:51}
//User = {[IdentifiableName: Id=80, Name=Гульнара Фаттахова]}