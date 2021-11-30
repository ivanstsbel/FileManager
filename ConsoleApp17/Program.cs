using System;
using System.IO;

namespace ConsoleApp17
{
    class Program
    {
        static void Main(string[] args)
        {
            string startcat = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\";
            string dirName = startcat;
            string proektdir = Environment.CurrentDirectory;


            int ov = 0;
            int strcur = 1;
            int i = 0;
            int kstr = 0;
            int kolv = 15;
            int ovl = kolv;

            bool min = false;

            while (true)
            {
                if (Directory.Exists(dirName))
                {

                    string[] dirs = Directory.GetDirectories(dirName);
                    string[] files = Directory.GetFiles(dirName);
                    string[] arr = new string[dirs.Length + files.Length];
                    i = 0;
                    foreach (string d in dirs)
                    {
                        arr[i] = d;
                        i++;
                    }

                    foreach (string f in files)
                    {
                        arr[i] = f;

                        i++;
                    }

                    Console.Clear();

                    kstr = i / kolv; //получаем  количество страниц
                    if ((i % kolv) != 0) kstr++; // если кол во файлов не делится на цело на кол во элементов на странице, то добавляем еще одну

                    if (i < kolv)           //если файлов меньше чем колво эл.на странице
                    {
                        ovl = i;            //то последняя итерация цикла = колву файлов
                        min = true;         //и ставим флаг
                    }
                    if (strcur == kstr && ovl != i - 1 && min == false && (i % kolv) != 0)  //если текущ.стр это послед. стр и последняя итерация цикла 
                    {                                                                       //не равна колво файлов-1 и флаг лож и колво файлов не деится на цело на колво элементов
                        ovl = ovl + (i % kolv) - kolv;
                        min = true;
                    }
                    if (min == true && strcur < kstr)

                    {
                        ovl = ovl + (kolv - (i % kolv));

                        min = false;
                    }

                    for (int o = ov; o < ovl; o++)
                        Console.WriteLine($"{o + 1}.{arr[o]}");
                    Console.WriteLine($"\nТекущая директория: {dirName} \t Страниц: {kstr} \t Текущая страница: {strcur} \t всего файлов/папок:{proektdir}");


                }

                if (!Directory.Exists(dirName))
                {
                    Console.WriteLine("Ошибка! директория не существует! Перенаправление на стартовый каталог ->");
                    System.Threading.Thread.Sleep(1500);
                    dirName = startcat;
                    continue;
                }
                    string next = Console.ReadLine();
                string[] nexts = next.Split();



                switch (nexts[0])
                {
                    case "mkdir":
                        var vrdir = new System.Text.StringBuilder();

                        for (int j = 1; j < nexts.Length; j++)
                        {
                            if (j == 1) vrdir.Append(dirName);
                            vrdir.Append(nexts[j].ToString() + " ");
                        }

                        if (!Directory.Exists(vrdir.ToString()))
                        {
                            Directory.CreateDirectory(vrdir.ToString());
                            if(strcur==kstr&&(i%kolv)!=0)
                            ovl++;
                            break;
                        }
                        if (Directory.Exists(vrdir.ToString()))
                        {
                            Console.WriteLine("Ошибка! Папка с таким именем уже сушествует!");
                            System.Threading.Thread.Sleep(1000);
                            break;
                        }
                        break;

                    case "rm":
                        var vrrm = new System.Text.StringBuilder();

                        for (int j = 1; j < nexts.Length; j++)
                        {
                            if (j == 1) vrrm.Append(dirName);
                            vrrm.Append(nexts[j].ToString() + " ");
                        }

                        if (Directory.Exists(vrrm.ToString()) || File.Exists(vrrm.ToString())) 
                        {
                            if (File.Exists(vrrm.ToString())) File.Delete(vrrm.ToString());
                            if(Directory.Exists(vrrm.ToString())) Directory.Delete(vrrm.ToString(),true);
                            if (i - 1 == kolv && strcur == kstr)
                            {
                                strcur--;
                                ovl--;
                                ov = ov - kolv;
                                break;
                            }
                            if(strcur == kstr)
                            ovl--;

                            break;
                        }
                        if (!Directory.Exists(vrrm.ToString())&& (!File.Exists(vrrm.ToString())))
                        {
                            Console.WriteLine("Ошибка! Папка/файл с таким именем не сушествует!");
                            System.Threading.Thread.Sleep(1000);
                            break;
                        }
                        break;

                    case "cp":
                        var vrcp = new System.Text.StringBuilder();

                        for (int j = 1; j < nexts.Length; j++)
                        {
                            if (j == 1) vrcp.Append(dirName);
                            vrcp.Append(nexts[j].ToString() + " ");
                        }

                        if (File.Exists(vrcp.ToString())) File.Copy(vrcp.ToString(), vrcp.ToString()+"1");
                        if (Directory.Exists(vrcp.ToString())) DirectoryCopy(vrcp.ToString(), vrcp + "(1)", true);
                        break;

                    case "cd":
                        var vrcd = new System.Text.StringBuilder();

                        for (int j = 1; j < nexts.Length; j++)
                        {
                            if (nexts[1] == "..")
                            {

                                string par = Directory.GetParent(dirName.Trim('\\')).FullName + "\\";
                                if (Directory.Exists(par))
                                    vrcd.Append(par.ToString());
                                if (!Directory.Exists(par)||par== proektdir)
                                {
                                    Console.WriteLine("Это корневая директория!");
                                    System.Threading.Thread.Sleep(1000);
                                }
                                break;
                            }

                            if (j == 1) vrcd.Append(dirName.ToString());
                            vrcd.Append(nexts[j].ToString());
                            if (!(j == nexts.Length - 1)) vrcd.Append(" ");
                            if ((j == nexts.Length - 1)) vrcd.Append("\\");

                        }

                        if (Directory.Exists(vrcd.ToString()))
                        {
                            dirName = vrcd.ToString();
                            strcur = 1;
                            ov = 0;
                            ovl = kolv;
                            min = false;
                            break;
                        }
                        if (!Directory.Exists(vrcd.ToString()))
                        {
                            Console.WriteLine("Не верно указана директория!");
                            System.Threading.Thread.Sleep(1000);
                            break;
                        }
                        break;

                    case "str":
                        if (nexts[1] == "+")
                        {
                            if (strcur == kstr)
                            {
                                Console.WriteLine("это последняя страница!");

                                System.Threading.Thread.Sleep(1000);
                                break;
                            }
                            if (strcur < kstr + 1)
                            {
                                strcur = strcur + 1;
                                ov = ov + kolv;
                                ovl = ovl + kolv;
                                break;
                            }

                        }
                        if (nexts[1] == "-")
                        {
                            if (strcur != 1)
                            {
                                strcur = strcur - 1;
                                ov = ov - kolv;
                                ovl = ovl - kolv;
                                break;
                            }
                            if (strcur == 1)
                            {
                                Console.WriteLine("это первая страница!");
                                System.Threading.Thread.Sleep(1000);
                            }
                        }
                        break;

                    default:
                        Console.WriteLine("Команда не распознана!");
                        System.Threading.Thread.Sleep(1000);
                        break;
                }

            }

        }
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }

    }
}
