using System;
using System.Collections.Generic;

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

            private MongoServer Server;
            private MongoDatabase Database;

            public Session(MongoServer server, MongoDatabase database, String sessionKey)
            {
                this.Time = DateTime.Now.ToLocalTime();
                this.Key = sessionKey;
                this.Values = new Dictionary<String, dynamic>();

                this.Server = server;
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
                using (this.Server.RequestStart(this.Database))
                {
                    MongoCollection<LazyBsonDocument> sessions = this.Database.GetCollection<LazyBsonDocument>("sessions");

                    sessions.Update(
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
        }

        private MongoClient mongoClient;
        private MongoServer mongoServer;
        private MongoDatabase mongoDatabase;

        public MongoDB()
        {
            this.mongoClient = new MongoClient("mongodb://localhost:27017");

            this.mongoServer = mongoClient.GetServer();

            this.mongoServer.Connect();

            if (this.mongoServer.State == MongoServerState.Connected)
            {
                this.mongoDatabase = this.mongoServer.GetDatabase("esserver");

                Console.WriteLine("mongo start success");
            }

            Console.WriteLine("mongo start end");
        }

        ~MongoDB()
        {
            if (this.mongoServer != null)
            {
                this.mongoServer.Disconnect();
            }
        }

        public ISession Add(String sessionKey)
        {
            using (this.mongoServer.RequestStart(this.mongoDatabase))
            {
                MongoCollection<LazyBsonDocument> sessions = this.mongoDatabase.GetCollection<LazyBsonDocument>("sessions");

                sessions.Update(
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
                    new MongoUpdateOptions
                    {
                        Flags = UpdateFlags.Upsert
                    }
                );

                Session session = new Session(this.mongoServer, this.mongoDatabase, sessionKey);

                return session;
            }
        }

        public Boolean Has(String sessionKey)
        {
            using (this.mongoServer.RequestStart(this.mongoDatabase))
            {
                MongoCollection<LazyBsonDocument> sessions = this.mongoDatabase.GetCollection<LazyBsonDocument>("sessions");

                LazyBsonDocument session = sessions.FindOne(
                    new QueryDocument
                    {
                        {
                            "Key",
                            sessionKey
                        }
                    }
                );

                return session != null;
            }
        }

        public ISession Get(String sessionKey)
        {
            using (this.mongoServer.RequestStart(this.mongoDatabase))
            {
                MongoCollection<LazyBsonDocument> sessions = this.mongoDatabase.GetCollection<LazyBsonDocument>("sessions");

                LazyBsonDocument document = sessions.FindOne(
                    new QueryDocument
                    {
                        {
                            "Key",
                            sessionKey
                        }
                    }
                );

                Session session = new Session(this.mongoServer, this.mongoDatabase, sessionKey);

                BsonDocument values = document.GetValue("Values").AsBsonDocument;

                foreach (String name in values.Names)
                {
                    session.Values.Add(name, values.GetValue(name).AsString);
                }

                return session;
            }
        }
    }
}
