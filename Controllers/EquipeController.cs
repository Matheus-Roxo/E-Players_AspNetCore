using System;
using System.IO;
using E_Players_AspNetCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Players_AspNetCore.Controllers
{
    // http://localhost:5000/Equipe
    [Route("Equipe")]
    public class EquipeController : Controller
    {

        // Criamos uma instância de equipeModel
        Equipe equipeModel = new Equipe();


        [Route("Listar")]
        public IActionResult Index()
        {

            // Listamos todas as equipes e enviamos para a View através da ViewBag
            ViewBag.Equipes = equipeModel.ReadAll();
            return View();

        }
        
        [Route("Cadastrar")]
        public IActionResult Cadastrar(IFormCollection form)
        {

            Equipe novaEquipe = new Equipe();
            
            novaEquipe.IdEquipe = Int32.Parse( form["IdEquipe"] );
            novaEquipe.Nome = form["Nome"];

            // upload inicio

            // Verificamos se o usuario selecionou um arquivo
            if(form.Files.Count > 0)
            {
                var file = form.Files[0];
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Equipes");
                
                if(!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                                            //localhost:5001                             Equipes    Imagem.jpg       
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/", folder, file.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                novaEquipe.Imagem = file.FileName;
            }
            else
            {
                novaEquipe.Imagem = "padrao.png";
            }


            //upload final


            // Chamamos o método Create para salvar a novaEquipe no CSV
            equipeModel.Create(novaEquipe);
            // Atualiza a lista de equipes na View
            ViewBag.Equipes = equipeModel.ReadAll();

            return LocalRedirect("~/Equipe/Listar");
        }

        [Route("{id}")]
        public IActionResult Excluir(int id)
        {
            equipeModel.Delete(id);
            ViewBag.Equipes = equipeModel.ReadAll();

            return LocalRedirect("~/Equipe/Listar");
        }
    }
}