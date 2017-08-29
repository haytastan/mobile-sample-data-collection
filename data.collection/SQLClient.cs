﻿using System;
using System.IO;
using SQLite;
using System.Collections.Generic;

namespace data.collection
{
    public class SQLClient
    {
        public static readonly SQLClient Instance = new SQLClient();
        readonly SQLiteConnection db;

        SQLClient()
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            const string file = "data.collection.db";

            db = new SQLiteConnection(Path.Combine(folder, file));

            // 'Create' ensures that it's created, does not recreate if exists
            db.CreateTable<Data>();
        }

        public void Insert(Data data)
        {
            db.Insert(data);
        }

        public List<Data> GetAll()
        {
            return db.Query<Data>("SELECT * FROM Data");
        }

        public void Delete(Data data)
        {
            db.Delete(data);
        }

        public void DeleteAll()
        {
            db.DeleteAll<Data>();
        }

        public void UpdateAll(List<Data> items)
        {
            db.UpdateAll(items);
        }
    }
}
