using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Driver;

namespace com.erinus.ESServer.Session.Store
{
    public class MongoDB : ISessionStore
    {
        public class Session : ISession
        {
            public DateTime Time;

            public String Key;

            public Dictionary<String, dynamic> Values;

            private IMongoClient Client;

            private IMongoDatabase Database;

            public Session(IMongoClient client, IMongoDatabase database, String sessionKey)
            {
                this.Time = DateTime.Now.ToLocalTime();

                this.Key = sessionKey;

                this.Values = new Dictionary<String, dynamic>();

                this.Client = client;

                this.Database = database;
            }

            public Boolean Has(String key)
            {
                return this.Values.ContainsKey(key);
            }

            public T Get<T>(String key) where T : class
            {
                T value = null;

                if (this.Values.ContainsKey(key))
                {
                    value = this.Values[key] as T;
                }

                return value;
            }

            public Boolean Set(String key, dynamic value)
            {
                IMongoCollection<BsonDocument> sessions = this.Database.GetCollection<BsonDocument>("sessions");

                sessions.UpdateOneAsync(

                    new QueryDocument
                    {
                        {
                            "Key",
                            this.Key
                        }
                    },
                    new UpdateDocument
                    {
                        {
                            "$set",
                            new BsonDocument
                            {
                                {
                                    "Values." + key,
                                    value
                                }
                            }
                        }
                    }

                );

                this.Values[key] = value;

                return true;
            }
        }

        private IMongoClient mongoClient;

        private IMongoDatabase mongoDatabase;

        public MongoDB()
        {
            this.mongoClient = new MongoClient("mongodb://localhost:27017");

            this.mongoDatabase = this.mongoClient.GetDatabase("esserver");
        }

        ~MongoDB()
        {
            
        }

        public ISession Add(String sessionKey)
        {
            IMongoCollection<BsonDocument> sessions = this.mongoDatabase.GetCollection<BsonDocument>("sessions");

            sessions.UpdateOneAsync(

                new QueryDocument
                {
                    {
                        "Key",
                        sessionKey
                    }
                },
                new UpdateDocument
                {
                    {
                        "$set",
                        new BsonDocument
                        {
                            {
                                "Time",
                                DateTime.Now.ToLocalTime()
                            },
                            {
                                "Key",
                                sessionKey
                            },
                            {
                                "Values",
                                new BsonDocument
                                {

                                }
                            }
                        }
                    }
                },
                new UpdateOptions
                {
                    IsUpsert = true
                }

            );

            Session session = new Session(this.mongoClient, this.mongoDatabase, sessionKey);

            return session;
        }

        public Boolean Has(String sessionKey)
        {
            IMongoCollection<BsonDocument> sessions = this.mongoDatabase.GetCollection<BsonDocument>("sessions");

            QueryDocument query = new QueryDocument {

                { "Key", sessionKey }

            };

            Task<IAsyncCursor<BsonDocument>> search = sessions.FindAsync<BsonDocument>(query);

            IAsyncCursor<BsonDocument> cursor = search.Result;

            Task<List<BsonDocument>> next = cursor.ToListAsync<BsonDocument>();

            return next.Result.Count != 0;
        }

        public ISession Get(String sessionKey)
        {
            IMongoCollection<BsonDocument> sessions = this.mongoDatabase.GetCollection<BsonDocument>("sessions");

            QueryDocument query = new QueryDocument {

                { "Key", sessionKey }

            };

            Task<IAsyncCursor<BsonDocument>> search = sessions.FindAsync<BsonDocument>(query);

            IAsyncCursor<BsonDocument> cursor = search.Result;

            Task<bool> next = cursor.MoveNextAsync();

            if (!next.Result)
            {
                return null;
            }

            Session session = new Session(this.mongoClient, this.mongoDatabase, sessionKey);

            IEnumerable<BsonDocument> batch = cursor.Current;

            IEnumerator<BsonDocument> documents = batch.GetEnumerator();

            documents.MoveNext();

            BsonDocument document = documents.Current;

            BsonDocument values = document.GetValue("Values").AsBsonDocument;

            foreach (String name in values.Names)
            {
                session.Values.Add(name, values.GetValue(name).AsString);
            }

            return session;
        }
    }
}
