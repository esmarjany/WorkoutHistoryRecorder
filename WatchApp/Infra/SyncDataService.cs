using App2.Droid.Provider;
using Newtonsoft.Json;
using Samsung.Sap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tizen.Wearable.CircularUI.Forms;
using WearableCompanion.Droid;
using WorkoutHistoryRecorder.Contract;
using WorkoutHistoryRecorder.Contract.Tools;

namespace WorkoutHistoryRecorder.WatchApp.Infra
{
    internal class SyncDataService //<T> where T : class
    {
        readonly SAPService _SAPService;
        public SyncDataService()
        {
            _SAPService = new SAPService();
            _SAPService.DataReceived += Connection_DataReceived;
            Connect();
        }

        public async void Connect()
        {
            if (await _SAPService.Connect())
                Toast.DisplayText("Connected");
            else
                Toast.DisplayText("Connection failed!");
        }

        private void Connection_DataReceived(object sender, DataReceivedEventArgs e)
        {
            var res = MySerializer.DeserializeObject<PhoneResult>(e.Data);
            if (res != null)
            {
                switch (res.CommandType)
                {
                    case CommandType.None:
                        break;
                    case CommandType.GetWorkoutList:
                        ListRecived(res);
                        break;
                    case CommandType.StoreRecord:
                        break;
                    case CommandType.Message:
                        break;
                    default:
                        break;
                }
            }

        }

        private void ListRecived(PhoneResult res)
        {
            var workouts = MySerializer.DeserializeObject<WorkoutMessage>(res.Result);
            StorageService.SetWorkoutTemplate(workouts.WorkoutTemplates);
            //StorageService.SetWorkout(workouts.Workouts);
        }

        public void SyncReords()
        {
            ReciveWorkout();
            SendRecords();
        }

        private void SendRecords()
        {
            var lastSyncDate = StorageService.GetLastSync();
            var param = MySerializer.SerializeObject(StorageService.GetWorkoutRecordByDate(lastSyncDate).ToList());
            try
            {
                _SAPService.SendBytes(MySerializer.SerializeObject(new WatchCommand(CommandType.StoreRecord, param)));
                StorageService.DeleteOlderThan(DateTime.Now.AddDays(-1));
            }
            catch (Exception ex)
            {
                Toast.DisplayText("SendRecords : "+ ex.Message);
            }
        }

        //public void SendText(string text)
        //{
        //    try
        //    {
        //        _SAPService.SendText(MySerializer.SerializeObject(new WatchCommand<T>(CommandType.Message, text)));
        //    }
        //    catch (Exception ex)
        //    {
        //        Toast.DisplayText("SendText" + ex.Message);
        //    }
        //}

        public void ReciveWorkout()
        {
            try
            {
                _SAPService.SendBytes(MySerializer.SerializeObject(new WatchCommand(CommandType.GetWorkoutList,null)));
            }
            catch (Exception ex)
            {
                MyLoger.Log(ex);
                Toast.DisplayText("ReciveWorkout : "+ ex.Message);
            }
        }
    }

    
}
