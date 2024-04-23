// See https://aka.ms/new-console-template for more information
using System;
using System.IO;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;
class Tarefas
{
    class Tarefa
    {
        public string Titulo { get; set; }
        public bool Concluida { get; set; }
        public string Descricao { get; set; }

        public Tarefa(string titulo, string descricao, bool concluida)
        {
            Titulo = titulo;
            Descricao = descricao;
            Concluida = false;
        }

    }
    class ListaDeTarefas
    {
        public List<Tarefa> Tarefas { get; set; }

        public ListaDeTarefas()
        {
            Tarefas = new List<Tarefa>();
        }

        public void AdicionarTarefa(Tarefa tarefa)
        {
            Tarefas.Add(tarefa);
        }

        public void listarTarefas()
        {
            int index = 1;
            foreach (Tarefa tarefa in Tarefas)
            {
                Console.WriteLine($"\nTarefa - {index++}");
                Console.WriteLine($"Título: {tarefa.Titulo}");
                Console.WriteLine($"Descrição: {tarefa.Descricao}");
                Console.WriteLine($"Concluída: {tarefa.Concluida} \n");
            }
        }
        public void EditarTarefa()
        {
            bool sucessoIndex;
            int index;
            do
            {

                Console.WriteLine("Digite o número da tarefa que deseja editar:");
                sucessoIndex = int.TryParse(Console.ReadLine(), out index);
                if (index > Tarefas.Count) Console.WriteLine("Tarefa não encontrada, digite um número válido \n");

            } while (index > Tarefas.Count || index == 0 || !sucessoIndex);

            string? titulo, descricao;
            do
            {
                Console.WriteLine("Digite o novo título da tarefa:");
                titulo = Console.ReadLine();
                Console.WriteLine("Digite a nova descrição da tarefa:");
                descricao = Console.ReadLine();
                Console.WriteLine();
            } while (string.IsNullOrEmpty(titulo) || string.IsNullOrEmpty(descricao));

            int concluida;
            bool sucessoConcluida;
            do
            {
                Console.WriteLine("Tarefa concluida? (Dígite apenas o número)");
                Console.WriteLine("1 - Sim");
                Console.WriteLine("2 - Não");
                sucessoConcluida = int.TryParse(Console.ReadLine(), out concluida);
                Console.WriteLine();
            } while (!sucessoConcluida || concluida > 2 || concluida == 0);


            Tarefas[index - 1].Titulo = titulo;
            Tarefas[index - 1].Descricao = descricao;
            Tarefas[index - 1].Concluida = concluida == 1 ? true : false;
        }

        public void ExcluirTarefa()
        {
            bool sucessoIndex;
            int index;
            int certo = 0;
            bool sucessoCerto = false;
            do
            {

                Console.WriteLine("Digite o número da tarefa que deseja excluir:");
                sucessoIndex = int.TryParse(Console.ReadLine(), out index);
                if (index > Tarefas.Count)
                {

                    Console.WriteLine("Tarefa não encontrada, digite um número válido \n");
                }
                else
                {

                    Console.WriteLine("Tarefa selecionada:\n");
                    Console.WriteLine($"Titulo: {Tarefas[index - 1].Titulo}");
                    Console.WriteLine($"Descrição: {Tarefas[index - 1].Descricao}");
                    Console.WriteLine($"Concluida?: {Tarefas[index - 1].Concluida}\n");

                    Console.WriteLine("Tem certeza que deseja excluir essa tarefa?\n");
                    Console.WriteLine("1 - Sim");
                    Console.WriteLine("2 - Não");
                    sucessoCerto = int.TryParse(Console.ReadLine(), out certo);
                    Console.WriteLine();
                }


            } while (index > Tarefas.Count || index == 0 || !sucessoIndex || !sucessoCerto || certo > 1 || certo <= 0);

            Tarefas.RemoveAt(index - 1);
            Console.WriteLine("Tarefa excluída \n");
        }
    }

    static void Main(string[] args)
    {
        string? path = $"{System.IO.Path.GetDirectoryName(
System.Reflection.Assembly.GetExecutingAssembly().Location)}\\tarefas.txt";
        
        ListaDeTarefas ListaTarefa = new ListaDeTarefas();
        ListaTarefa = CarregarTarefas(path);

        Console.WriteLine("Bem vindo ao sistema de tarefas");

        int option;
        do
        {
            option = Menu();
            if (option == 1)
            {
                Tarefa tarefa = AddTask();
                ListaTarefa.AdicionarTarefa(tarefa);

            }
            else if (option == 2)
            {
                ListaTarefa.EditarTarefa();
            }
            else if (option == 3)
            {

                ListaTarefa.listarTarefas();
            }
            else if (option == 4)
            {

                ListaTarefa.ExcluirTarefa();
            }
            else
            {
                SalvarTarefas(path, ListaTarefa);
                return;
            }

        } while (option != 5);
    }

    static int Menu()
    {

        int resp;
        bool success;
        do
        {
            Console.WriteLine("Escolha uma opção:");
            Console.WriteLine("1 - Adicionar tarefa");
            Console.WriteLine("2 - Editar tarefa");
            Console.WriteLine("3 - Listar tarefas");
            Console.WriteLine("4 - Excluir tarefa");
            Console.WriteLine("5 - Sair");
            success = int.TryParse(Console.ReadLine(), out resp);
        } while (resp > 5 || resp == 0 || !success);

        Console.WriteLine();
        return resp;
    }

    static Tarefa AddTask()
    {

        string? titulo, descricao;
        do
        {
            Console.WriteLine("Digite o título da tarefa:");
            titulo = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("Digite a descrição da tarefa:");
            descricao = Console.ReadLine();

            if (string.IsNullOrEmpty(titulo) || string.IsNullOrEmpty(descricao)) Console.WriteLine("--Título e descrição são obrigatórios-- \n");

        } while (string.IsNullOrEmpty(titulo) || string.IsNullOrEmpty(descricao));

        Console.WriteLine();
        Tarefa tarefa = new Tarefa(titulo, descricao,false);
        Console.WriteLine("Tarefa criada \n");
        return tarefa;
    }

    static ListaDeTarefas CarregarTarefas(string path)
    {

        
        ListaDeTarefas ListaTarefa = new ListaDeTarefas();
        
        do
        {
            if (File.Exists(path))
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    string line;
                    int index = 0;
                    string[] tarefas = new string[3];
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Equals("--QuebraTarefa--"))
                        {
                            Console.WriteLine(tarefas[0]);
                            Tarefa tarefa = new Tarefa(tarefas[0], tarefas[1], tarefas[2] == "False"?false:true );
                            ListaTarefa.AdicionarTarefa(tarefa);
                            index =0;
                        }
                        else
                        {
                            Console.WriteLine(line);
                            tarefas[index++] = line;
                        }
                    }
                }
            }
            else
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    
                    writer.Write("");
                }
            }

        } while(!File.Exists(path));
        
        return ListaTarefa;
    }

    static void SalvarTarefas(string path, ListaDeTarefas data)
    {
        string test="";
        foreach (Tarefa tarefa in data.Tarefas)
        {
            test += $"{tarefa.Titulo}\n";
            test += $"{tarefa.Descricao} \n";
            test += $"{tarefa.Concluida} \n";
            test += "--QuebraTarefa--\n";
        }

        using (StreamWriter writer = new StreamWriter(path))
        {
            writer.Write(test);
        }
    }
}