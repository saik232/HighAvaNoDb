using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Org.Apache.Zookeeper.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using ZooKeeperNet;

namespace HighAvaNoDb.Tests.HelperForZK
{
    /// <summary>
    /// Don't use it. Watch the watches.
    /// </summary>
    [TestClass]
    public class HelperForZK
    {
        IZooKeeper zookeeper = new ZooKeeper("127.0.0.1:2181", new TimeSpan(0, 60, 0), null);

        [TestMethod]
        public void DeleteAllNode( )
        {
            DeleteNode(null);
        }


        public void DeleteNode(string path)
        {
            //IEnumerable<string> names = zookeeper.GetChildren(path == null ? "/" : path, false);
            //if (names != null)
            //{
            //    foreach (var name in names)
            //    {
            //        DeleteNode(path + "/" + name);
            //    }
            //  Stat  tat=  zookeeper.Exists(path, null);
            //    zookeeper.Delete(path, tat.Version);
            //}
            //else
            //{
            //    Stat tat = zookeeper.Exists(path, null);
            //    zookeeper.Delete(path, tat.Version);
            //    return;
            //}
        }






        [TestMethod]
        public void Test()
        {
            ZKTree tree = new ZKTree();

            GetZKTree(tree);

            Debug.WriteLine(ConvertJsonString(tree));
        }

        public void GetZKTree(ZKTree tree)
        {

            IEnumerable<string> names = zookeeper.GetChildren(tree.Path==null?"/": tree.Path, false);
            if (names != null)
            {
                tree.Children = new List<ZKTree>();
                foreach (var name in names)
                {
                    ZKTree child = new ZKTree();
                    child.Name = name;
                    child.Path = tree.Path + "/" + name ;
                    child.Data = zookeeper.GetData(child.Path,false,null);
                    tree.Children.Add(child);
                    GetZKTree(child);
                }
            }
            else
            {
                return;
            }
        }
        private string ConvertJsonString(object obj)
        {
            JsonSerializer serializer = new JsonSerializer();
            if (obj != null)
            {
                StringWriter textWriter = new StringWriter();
                JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4,
                    IndentChar = ' '
                };
                serializer.Serialize(jsonWriter, obj);
                return textWriter.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

    }

    public class ZKTree
    {
        public string Path { set; get; }
        public string Name { set; get; }
        public byte[] Data { set; get; }
        public ICollection<ZKTree> Children { set; get; }

        public bool IsLeaf
        {
            get
            {
                return Children == null || Children.Count == 0;
            }
        }

        public int ChildrenCount
        {
            get { return Children == null ? 0 : Children.Count; }
        }
    }
}
