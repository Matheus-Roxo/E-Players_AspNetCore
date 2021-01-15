using System.Collections.Generic;
using System.IO;
using EPlayers_AspNetCore.Models;

namespace E_Players_AspNetCore.Models
{
    public class Equipe : EPlayersBase , Interfaces.IEquipe
    {
        
        public int IdEquipe { get; set; }
        public string Nome { get; set; }
        public string Imagem { get; set; }

        private const string PATH = "Database/Equipe.csv";

        public Equipe()
        {
            CreateFolderAndFile(PATH);
        }

        // Criamos o método para preparar a linha do CSV
        public string Prepare(Equipe e){
            return $"{e.IdEquipe};{e.Nome};{e.Imagem}";
        }


        public void Create(Equipe e)
        {
            // Preparamos um Array de string para o método AppendAllLines
            string[] linhas = { Prepare(e) };
            // Acrescentamos a nova linha
            File.AppendAllLines(PATH,linhas);

        }

        public void Delete(int id)
        {
            List<string> linhas = ReadAllLinesCSV(PATH);

            // 2;SNK;snk.jpg
            // Removemos as linhas com o código comparado
            // ToString -> Converte para texto (string)
            linhas.RemoveAll(x => x.Split(";")[0] == id.ToString());

            
        
            // Reescreve o csv com a lista alterada
            RewriteCSV(PATH, linhas);
        }

        public List<Equipe> ReadAll()
        {
            List<Equipe> equipes = new List<Equipe>();
            // Lemos todas as linhas do csv
            string[] linhas = File.ReadAllLines(PATH); 

            foreach(string item in linhas)
            {
                // 1;VivoKeyd;vivo.jpg
                // [0 ] = 1
                // [1] = VivoKeyd
                // [2] = vivo.jpg

                string[] linha = item.Split(";");

                Equipe novaEquipe = new Equipe();
                novaEquipe.IdEquipe = int.Parse( linha[0] );
                novaEquipe.Nome = linha[1];
                novaEquipe.Imagem = linha[2];

                equipes.Add(novaEquipe);
            }

        
            return equipes;        
        }

        public void Update(Equipe e)
        {
            List<string> linhas = ReadAllLinesCSV(PATH);

            // 2;SNK;snk.jpg
            // Removemos as linhas com o código comparado
            linhas.RemoveAll(x => x.Split(";")[0] == e.IdEquipe.ToString());

            // Adicionamos na lista a equipe alterada
            linhas.Add( Prepare(e) );
            
            // Reescreve o csv com a lista alterada
            RewriteCSV(PATH, linhas);
        }
    }
}