using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace PlanetXTest
{
    public partial class Form1 : Form
    {
        IList<IList<Object>> TaskCollection;
        CheckBox[] _checkBoxes = new CheckBox[100];
        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "Work Tasks";

        public Form1(IList<IList<Object>> values)
        {
            TaskCollection = values;
            InitializeComponent();
            label2.Text = SetDate();
            SetUp();
            

        }
        private void SetUp()
        {
            string value;
            int TotalTasks;
            CheckBox box;
            int formHeight = 161;
            int button1Position = 76;


            try { TotalTasks = TaskCollection.Count; }
            catch (NullReferenceException) { TotalTasks = 0; }


            for (int i = 1; i < TotalTasks; i++)
            {
                //Alters the form length and Update button position
                formHeight = formHeight + 30;
                button1Position = button1Position + 30;
                this.Size = new Size(300, formHeight);
                button1.Location = new System.Drawing.Point(12, button1Position);
                

                value = TaskCollection[i][0].ToString();
                box = new CheckBox();
                box.Tag = i.ToString();
                box.Text = value;
                box.AutoSize = true;
                box.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
                box.Location = new Point(12, 76 + (i-1) *30);
                this.Controls.Add(box);
                _checkBoxes[i] = box;
            }
        }
        public string SetDate()
        {
             return(DateTime.Now.ToString("dddd"));
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            string value;
            for (int i = 0; i < TaskCollection.Count; i++)
            {
                try { value = TaskCollection[i][0].ToString(); }
                catch (ArgumentOutOfRangeException) { goto DoneWithList; }
                Console.Write(value + " ");
                Console.WriteLine("test");
            }
            DoneWithList:;
        }

        private void addTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        static IList<IList<Object>> ConnectToSheets(string Weekday)
        {
            UserCredential credential;

            using (var stream =
                new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.com-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes, "user", CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            String spreadsheetId = "1ZhMmd8DAGt28MWlmy1QrWHIm3d0ln3Zi4x66Qs4bmJY";
            String sheet = DateTime.Now.ToString("dddd");
            String range = DateTime.Now.ToString("dddd") + "!A1:M10";

            SpreadsheetsResource.ValuesResource.GetRequest.DateTimeRenderOptionEnum dateTimeRenderOption = (SpreadsheetsResource.ValuesResource.GetRequest.DateTimeRenderOptionEnum)0;  // TODO: Update placeholder value.
            SpreadsheetsResource.ValuesResource.GetRequest.ValueRenderOptionEnum valueRenderOption = (SpreadsheetsResource.ValuesResource.GetRequest.ValueRenderOptionEnum)0;  // TODO: Update placeholder value.
            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, range);
            request.ValueRenderOption = valueRenderOption;
            request.DateTimeRenderOption = dateTimeRenderOption;

            Google.Apis.Sheets.v4.Data.ValueRange response = request.Execute();

            Console.WriteLine(JsonConvert.SerializeObject(response));
            IList<IList<Object>> values = response.Values;
            return (values);
        }
    }
}
