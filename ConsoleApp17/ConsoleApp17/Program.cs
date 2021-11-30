using System;
using System.IO;
using System.Configuration;

namespace ConsoleApp17
{
    class Program
    {
        static void Main(string[] args)
        {
            string startcat = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\"; //задаем стартовый каталог
            string dirName = startcat;

            if (!string.IsNullOrEmpty(Settings1.Default.posdir)) dirName = Settings1.Default.posdir; // если найден файл конфигурации берем директорию оттуда
            string proektdir = Environment.CurrentDirectory;


            int ov = 0;                                         //задаем основные необходимые переменные
            int strcur = 1;
            int i = 0;
            int kstr = 0;
            int kolv = Settings1.Default.kolvstr;
            int ovl = kolv;

            bool min = false;
            bool stop = true;


            while (true)                            //задаем бесконечныq цикл
            {
                if (Directory.Exists(dirName))  //проверяем существует ли директория и если да сразу сохраняем ее в настройки
                {
                    Settings1.Default.posdir = dirName;
                    Settings1.Default.Save();
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


                    for (int o = ov; o < ovl; o++)              //выводим энное количество элементов на экран и всю необходиму информацию
                        Console.WriteLine($"{o + 1}.{arr[o]}");
                    Console.WriteLine("----------------------------------------------------------------------------------------------------");
                    Console.WriteLine($"Текущая директория: {dirName} \nСтраниц: {kstr} \t Текущая страница: {strcur} \t всего файлов/папок:{i} \t количество элементов на странице {kolv}");
                    Console.WriteLine("----------------------------------------------------------------------------------------------------");

                }
                stop = false;
                if (!Directory.Exists(dirName))
                {
                    Console.WriteLine("Ошибка! директория не существует! Перенаправление на стартовый каталог ->");
                    System.Threading.Thread.Sleep(1500);
                    dirName = startcat;
                    continue;
                }

                string next = Console.ReadLine();       //ждем следующую команду
                string[] nexts = next.Split();



                switch (nexts[0])                       //парсер команд
                {
                    case "mkdir":
                        var vrdir = new System.Text.StringBuilder();

                        for (int j = 1; j < nexts.Length; j++)
                        {
                            if (j == 1) vrdir.Append(dirName);
                            vrdir.Append(nexts[j].ToString() + " ");
                        }

                        try
                        {
                            if (!Directory.Exists(vrdir.ToString()))
                            {
                                Directory.CreateDirectory(vrdir.ToString());
                                if (strcur == kstr && (i % kolv) != 0)
                                    ovl++;
                                Console.WriteLine($"Каталог создан!");
                                System.Threading.Thread.Sleep(1000);
                                break;
                            }
                        }
                        catch (UnauthorizedAccessException)
                        {
                            Console.WriteLine("Ошибка! Не удалось создать каталог! Недостаточно прав!");
                            System.Threading.Thread.Sleep(1000);
                            break;
                        }

                        catch
                        {
                            Console.WriteLine("Ошибка! Не удалось создать каталог!");
                            System.Threading.Thread.Sleep(1000);
                            break;
                        }

                        if (Directory.Exists(vrdir.ToString()))
                        {
                            if ((vrdir.ToString()).Trim(' ') == dirName)
                            {
                                Console.WriteLine("Введите название каталога!");
                                System.Threading.Thread.Sleep(1000);
                                break;
                            }
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

                        try
                        {
                            if (Directory.Exists(vrrm.ToString()) || File.Exists(vrrm.ToString()))
                            {


                                if (File.Exists(vrrm.ToString())) File.Delete(vrrm.ToString());
                                if (Directory.Exists(vrrm.ToString()))
                                {
                                    if ((vrrm.ToString()).Trim(' ') == dirName)
                                    {
                                        Console.WriteLine("Команда не распознана!");
                                        System.Threading.Thread.Sleep(1000);
                                        break;
                                        /*Console.WriteLine("Вы уверены что хотите удалить текущую директорию? Введите yes или no");
                                        string answ = Console.ReadLine();
                                        if (answ == "no") break;
                                        if(answ!="yes"&& answ != "no")
                                        {
                                            Console.WriteLine("Команда не распознана!");
                                            System.Threading.Thread.Sleep(1000);
                                            break;
                                        }
                                        if(answ=="yes") dirName = Directory.GetParent(dirName.Trim('\\')) + "\\"; */

                                    }
                                    Directory.Delete(vrrm.ToString(), true);
                                }
                                if (i - 1 == kolv && strcur == kstr)
                                {
                                    strcur--;
                                    ovl--;
                                    ov = ov - kolv;
                                    break;
                                }
                                if (strcur == kstr)
                                    ovl--;

                                break;
                            }
                        }
                        catch (UnauthorizedAccessException)
                        {
                            Console.WriteLine("Ошибка! Недостаточно прав для удаления файла!");
                            System.Threading.Thread.Sleep(1000);
                            break;
                        }
                        if (!Directory.Exists(vrrm.ToString()) && (!File.Exists(vrrm.ToString())))
                        {
                            Console.WriteLine("Ошибка! Папка/файл с таким именем не существует!");
                            System.Threading.Thread.Sleep(1000);
                            break;
                        }
                        break;

                    case "cp":
                        try
                        {
                            string ot;
                            string to;
                            var vrcp = new System.Text.StringBuilder();
                            var vrcpto = new System.Text.StringBuilder();
                            bool dirto = false;
                            for (int j = 1; j < nexts.Length; j++)
                            {
                                if (nexts[j] == "-")
                                {
                                    dirto = true;
                                    continue;
                                }

                                if (dirto == false)
                                {
                                    vrcp.Append(nexts[j].ToString());
                                    if (nexts[j + 1] != "-") vrcp.Append(" ");
                                }
                                if (dirto)
                                {
                                    vrcpto.Append(nexts[j].ToString());
                                    if (j != nexts.Length - 1) vrcpto.Append(" ");

                                }
                            }
                            to = vrcpto.ToString() + "\\" + vrcp.ToString();
                            ot = dirName + vrcp.ToString();


                            //Console.WriteLine($"{ot}  {to}");
                            // System.Threading.Thread.Sleep(3000);

                            if (File.Exists(ot))
                            {
                                if (File.Exists(to))
                                {
                                    Console.WriteLine($"Файл с таким именем уже существует!");
                                    System.Threading.Thread.Sleep(1000);
                                    break;
                                }
                                File.Copy(ot, to);
                                Console.WriteLine($"Файл скопирован!");
                                System.Threading.Thread.Sleep(1000);
                            }
                            if (Directory.Exists(ot))
                            {
                                if (Directory.Exists(to))
                                {
                                    Console.WriteLine($"Каталог с таким именем уже существует!");
                                    System.Threading.Thread.Sleep(1000);
                                    break;
                                }
                                DirectoryCopy(ot + "\\", to + "\\", true);
                                Console.WriteLine($"Каталог скопирован!");
                                System.Threading.Thread.Sleep(1000);
                            }
                            else
                            {
                                Console.WriteLine("копирование не удалось!");
                                System.Threading.Thread.Sleep(1000);
                            }
                            break;
                        }
                        catch (IndexOutOfRangeException)
                        {
                            Console.WriteLine("Некоректно введена команда!");
                            System.Threading.Thread.Sleep(1000);
                            break;
                        }
                        catch 
                        {
                            Console.WriteLine("Копирование не удалось!");
                            System.Threading.Thread.Sleep(1000);
                            break;
                        }

                    case "cd":
                        var vrcd = new System.Text.StringBuilder();

                        for (int j = 1; j < nexts.Length; j++)
                        {
                            if (nexts[1] == "..")
                            {

                                string par = Directory.GetParent(dirName.Trim('\\')).ToString() + "\\";
                                if (Directory.Exists(par) && par != Directory.GetParent(proektdir.Trim('\\')).FullName + "\\")
                                    vrcd.Append(par.ToString());
                                if (!Directory.Exists(par) || par == Directory.GetParent(proektdir.Trim('\\')).FullName + "\\")
                                {
                                    Console.WriteLine("Это корневая директория!");
                                    System.Threading.Thread.Sleep(1000);
                                    stop = true;
                                    break;

                                }
                                break;
                            }
                            if (nexts[1] == "abs")
                            {
                                if (j == 1) continue;
                                vrcd.Append(nexts[j].ToString());
                                if (!(j == nexts.Length - 1)) vrcd.Append(" ");
                                if ((j == nexts.Length - 1)) vrcd.Append("\\");

                            }

                            else
                            {
                                if (j == 1) vrcd.Append(dirName.ToString());
                                vrcd.Append(nexts[j].ToString());
                                if (!(j == nexts.Length - 1)) vrcd.Append(" ");
                                if ((j == nexts.Length - 1)) vrcd.Append("\\");
                            }

                        }

                        if (stop == true) break;
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
                            Console.WriteLine($"Не верно указана директория: {vrcd}");
                            System.Threading.Thread.Sleep(1000);
                            break;
                        }
                        break;

                    case "str":
                        try
                        {
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
                            else
                            {
                                Console.WriteLine("Некоректно введена команда!");
                                System.Threading.Thread.Sleep(1000);
                            }
                            break;
                        }
                        catch
                        {
                            Console.WriteLine("Некоректно введена команда!");
                            System.Threading.Thread.Sleep(1000);
                            break;
                        }
                    case "info":
                        var vrin = new System.Text.StringBuilder();

                        for (int j = 1; j < nexts.Length; j++)
                        {
                            if (j == 1) vrin.Append(dirName);
                            vrin.Append(nexts[j].ToString() + " ");
                        }

                        
                        
                        if (File.Exists(vrin.ToString()))
                        {
                            FileInfo fi = new FileInfo(vrin.ToString());
                            Console.WriteLine(String.Format($"Информация о файле: {fi.Name} \nРазмер: {fi.Length} bits\nСоздан: {fi.CreationTime}\nПоследнее изменение: {fi.LastWriteTime}\nАтрибуты: {fi.Attributes}")); 
                            Console.ReadKey();
                            break;
                        }
                        if (Directory.Exists(vrin.ToString()))
                        {
                            DirectoryInfo di = new DirectoryInfo(vrin.ToString());
                            Console.WriteLine($"Информация о каталоге: {di.Name} \nСоздан: {di.CreationTime}\nПоследнее изменение: {di.LastWriteTime} \nАтрибуты: {di.Attributes}");
                            Console.ReadKey();
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Файл или каталог не найден!");
                            System.Threading.Thread.Sleep(1000);
                            break;
                        }

                    case "help":
                        Console.WriteLine("Команды файл менеджера:\ncd относительная_категория или cd .. - перемещение по файловой системе \ncd abs абсолютная_категория - перемещение по указаной директории\nrm название_директории_или_файла - удаление каталога/файла\nmkdir название_каталога - создание каталога\ncp файл_или_каталог - абсолютный путь - копирование файла/каталога в указанную директорию\nstr +/str- - пролистывание страниц в текущей директории\nset page n - натройка кол-ва выводимых элементов на странице\ninfo название_каталога_или_файла - информация о каталоге или файле\nhelp - руководство ");
                        Console.ReadKey();
                        break;
                        
                    case "set":
                        try
                        {
                            int pag;
                            if (nexts[1] == "page" && nexts.Length == 3)
                            {
                                if (int.TryParse(nexts[2], out pag))
                                {
                                    kolv = pag;
                                    Settings1.Default.kolvstr = pag;
                                    Settings1.Default.Save();
                                    Console.WriteLine("Настройки изменены!");
                                    System.Threading.Thread.Sleep(1000);
                                    strcur = 1;
                                    ov = 0;
                                    if (i < kolv) ovl = i;
                                    if (i > kolv) ovl = kolv;

                                    min = false;
                                }
                                if (!int.TryParse(nexts[2], out pag))
                                {
                                    Console.WriteLine("Команда не распознана!");
                                    System.Threading.Thread.Sleep(1000);
                                }
                            }
                            if (nexts.Length < 3)
                            {
                                Console.WriteLine("Команда не распознана!");
                                System.Threading.Thread.Sleep(1000);
                            }
                            break;
                        }
                        catch
                        {
                            Console.WriteLine("Некоректно введена команда!");
                            System.Threading.Thread.Sleep(1000);
                            break;
                        }

                    default:
                        Console.WriteLine("Команда не распознана!");
                        System.Threading.Thread.Sleep(1000);
                        break;
                }

            }

        }
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs) //задаем отдельный метод для копирования
        {
           
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

      
            Directory.CreateDirectory(destDirName);


            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }


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
