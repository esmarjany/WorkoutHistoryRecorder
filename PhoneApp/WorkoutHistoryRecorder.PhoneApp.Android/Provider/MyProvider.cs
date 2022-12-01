using System;
using System.Collections.Generic;
using System.Linq;
using Android.Widget;
using App2;
using App2.Droid.Provider;
using Newtonsoft.Json;
using WorkoutHistoryRecorder.Contract;
using WorkoutHistoryRecorder.Contract.Tools;

namespace WearableCompanion.Droid
{
    public class MyProvider: ProviderService
    {
        private async void SendList(WatchCommand command)
        {
            var lasySycnDatetiem = MySerializer.DeserializeObject<DateTime>(command.Argument);
            var workouts = await App.Database.GetAllAsync<Workout>();
            workouts = workouts.Where(x => x.UpdateDateTime >= lasySycnDatetiem || x.CreateDateTime >= lasySycnDatetiem).ToList();
            var workoutTemplate = await App.Database.GetAllAsync<WorkoutTemplate>();
            var message = new WorkoutMessage { WorkoutTemplates = workoutTemplate/*,Workouts=workouts*/ };
            var res = MySerializer.SerializeObject(new PhoneResult(CommandType.GetWorkoutList, MySerializer.SerializeObject(message)));
            //using (var memoryStream = new MemoryStream())
            //{
            //    ProtoBuf.Serializer.Serialize(memoryStream, new PhoneResult(CommandType.GetWorkoutList, MySerializer.SerializeObject(message)));
            //    var res = memoryStream.ToArray();
            SendData(res);
            //}
        }

        private void MSocketServiceProvider_OnDataRecived(byte[] bytes)
        {
            // Check received data 
            string message = System.Text.Encoding.Unicode.GetString(bytes);
#if DEBUG
            Console.WriteLine("Received: ", message);
            Toast.MakeText(this, "Received: " + message, ToastLength.Long);
#endif

            var whatcCommand = MySerializer.DeserializeObject<WatchCommand>(bytes);
            if (whatcCommand != null)
            {
                switch (whatcCommand.CommandType)
                {
                    case CommandType.None:
                        break;
                    case CommandType.GetWorkoutList:
                        SendList(whatcCommand);
                        break;
                    case CommandType.StoreRecord:
                        SroreWorkoutRecords(whatcCommand);
                        break;
                    case CommandType.Message:
                        ShowMessage(whatcCommand);
                        break;
                    default:
                        break;
                }
            }
        }

        private void ShowMessage(WatchCommand whatcCommand)
        {
            Toast.MakeText(Android.App.Application.Context, whatcCommand.StringArgument, ToastLength.Long);
        }

        private void SroreWorkoutRecords(WatchCommand command)
        {
            var args = MySerializer.DeserializeObject<List<WorkoutRecord>>(command.Argument);
            foreach (var item in args)
                App.Database.SaveAsync(item);
        }


    }
}