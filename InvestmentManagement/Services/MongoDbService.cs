using System;
using System.IO;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using CsvHelper;
using System.Globalization;
using System.Linq;

namespace InvestmentManagement.Services
{
 public class MongoDbService

  {
    public static void LoadCsvFilesToDb(string csvFilePath, string collectionName)
    {
      string connectionString = "mongodb://kasim:MongoDB20admin@192.168.178.54:27017";
      MongoClient client = new MongoClient(connectionString);
      IMongoDatabase database = client.GetDatabase("crypto_trade_infos");

      //string directoryPath = @"C:\Users\Kasim\OneDrive - rfh-campus.de\Finanzen\Investment\Cryptos\Dokumente";

      //ProcessCsvFiles(directoryPath, database);

      //string collectionName = "network_transactions";
      //string csvFilePath = @"C:\path\to\your\csvfile.csv";

      List<NetworkTxnInfo> transactions = new List<NetworkTxnInfo>();

      using (var reader = new StreamReader(csvFilePath))
      using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
      {
        var records = csv.GetRecords<NetworkTxnInfo>();
        transactions = records.ToList();
      }
      var collection = database.GetCollection<NetworkTxnInfo>(collectionName);

      collection.InsertMany(transactions);

      Console.WriteLine("CSV data imported to MongoDB.");
    }

    public void LoadListToDb<T>(List<T> list, string collectionName)//string csvFilePath, string collectionName)
    {
      string connectionString = "mongodb://kasim:MongoDB20admin@192.168.178.54:27017";
      MongoClient client = new MongoClient(connectionString);
      IMongoDatabase database = client.GetDatabase("crypto_trade_infos");

      //string directoryPath = @"C:\Users\Kasim\OneDrive - rfh-campus.de\Finanzen\Investment\Cryptos\Dokumente";

      //ProcessCsvFiles(directoryPath, database);

      //string collectionName = "network_transactions";
      //string csvFilePath = @"C:\path\to\your\csvfile.csv";

      //List<NetworkTxnInfo> transactions = new List<NetworkTxnInfo>();

      //using (var reader = new StreamReader(csvFilePath))
      //using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
      //{
      //  var records = csv.GetRecords<NetworkTxnInfo>();
      //  transactions = records.ToList();
      //}
      var collection = database.GetCollection<T>(collectionName);

      collection.InsertMany(list);

      Console.WriteLine("CSV data imported to MongoDB.");
    }

    static void ProcessCsvFiles(string directory, IMongoDatabase db)
    {
      var directories = Directory.EnumerateDirectories(directory, "*", SearchOption.AllDirectories);
      foreach (var dir in directories)
      {
        var files = Directory.EnumerateFiles(dir, "*.csv");
        foreach (var file in files)
        {
          if (CheckFile(file))
          {
            string collectionName = ConvertDirPathToCollectionName(file, directory);
            InsertCsvDataToMongoDB(file, db.GetCollection<BsonDocument>(collectionName));
          }
        }
      }
    }

    static bool CheckFile(string file)
    {
      return file.EndsWith("Alle Transaktionen.csv");
    }

    static string ConvertDirPathToCollectionName(string filePath, string rootPath)
    {
      string collectionName = filePath.Replace(rootPath + "\\", "").Replace("\\", "_").Replace(" ", "").Replace("-", "").Replace(".csv", "");
      return collectionName;
    }

    static void InsertCsvDataToMongoDB(string file, IMongoCollection<BsonDocument> collection)
    {
      using (var reader = new StreamReader(file))
      {
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
          //csv.Configuration.HasHeaderRecord = true;
          var delim = csv.Configuration.Delimiter;
          //csv.Configuration.RegisterClassMap<MyClassMap>(); // You would need to define your own CsvClassMap to map CSV columns to your model

          var records = csv.GetRecords<dynamic>().ToList();
          List<BsonDocument> bsonDocs = new List<BsonDocument>();

          foreach (var record in records)
          {
            var document = record.ToBsonDocument();
            bsonDocs.Add(document);
          }

          collection.InsertMany(bsonDocs);
        }
      }
    }
  }
}




