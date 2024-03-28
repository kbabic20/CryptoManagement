using System;
using System.IO;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using CsvHelper;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

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
      var collection = database.GetCollection<BsonDocument>(collectionName);// database.GetCollection<T>(collectionName);

      //collection.InsertMany(list);

      var models = new List<WriteModel<BsonDocument>>();
      foreach (var item in list)
      {
        var doc = ToBsonDocument(item);

        // Erstelle einen Filter, der jedes Feld in doc berücksichtigt
        var filters = new List<FilterDefinition<BsonDocument>>();
        foreach (var element in doc)
        {
          filters.Add(Builders<BsonDocument>.Filter.Eq(element.Name, element.Value));
        }
        var filter = Builders<BsonDocument>.Filter.And(filters);

        var update = new BsonDocument("$set", doc);
        var updateOneModel = new UpdateOneModel<BsonDocument>(filter, update) { IsUpsert = true };
        models.Add(updateOneModel);

      }

      collection.BulkWrite(models);

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
    public static BsonDocument ToBsonDocument<T>(T item)
    {
      var document = new BsonDocument();
      foreach (PropertyInfo prop in typeof(T).GetProperties())
      {
        var value = prop.GetValue(item, null);
        if (value != null)
        {
          document[prop.Name] = BsonValue.Create(value);
        }
      }
      return document;
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

          var models = new List<WriteModel<BsonDocument>>();

          foreach (var record in records)
          {
            var doc = ToBsonDocument(record);

            // Erstelle einen Filter, der jedes Feld in doc berücksichtigt
            var filters = new List<FilterDefinition<BsonDocument>>();
            foreach (var element in doc)
            {
              filters.Add(Builders<BsonDocument>.Filter.Eq(element.Name, element.Value));
            }
            var filter = Builders<BsonDocument>.Filter.And(filters);

            var update = new BsonDocument("$set", doc);
            var updateOneModel = new UpdateOneModel<BsonDocument>(filter, update) { IsUpsert = true };
            models.Add(updateOneModel);
          }

          //collection.InsertMany(bsonDocs);

          try
          {
            var result = collection.BulkWrite(models);
            Console.WriteLine($"{result.InsertedCount} Dokumente wurden erfolgreich eingefügt.");
          }
          catch (MongoBulkWriteException<BsonDocument> ex)
          {
            foreach (var error in ex.WriteErrors)
            {
              if (error.Category == ServerErrorCategory.DuplicateKey)
              {
                Console.WriteLine($"Duplikatschlüssel-Fehler für Dokument: {models[error.Index].ToBsonDocument()} - {error.Message}");
              }
            }
          }
        }
      }
    }

    static string GenerateChecksum<T>(T item)
    {
      StringBuilder dataToHash = new StringBuilder();
      foreach (PropertyInfo prop in typeof(T).GetProperties())
      {
        var value = prop.GetValue(item, null);
        if (value != null)
        {
          dataToHash.Append(value.ToString());
        }
      }

      using (SHA256 sha256Hash = SHA256.Create())
      {
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(dataToHash.ToString()));

        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++)
        {
          builder.Append(bytes[i].ToString("x2"));
        }
        return builder.ToString();
      }
    }
  }
}




