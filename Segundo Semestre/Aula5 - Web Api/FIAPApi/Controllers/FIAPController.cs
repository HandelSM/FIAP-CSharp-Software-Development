using FIAPApi.Models;
using FIAPOracleEF.Database;
using FIAPOracleEF.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIAPApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FIAPController : ControllerBase
    {
        private AppDbContext _db = new AppDbContext();
        private GenericRepository<Aluno>? _alunosRepo;

        public FIAPController()
        {
            _alunosRepo = new GenericRepository<Aluno>(_db);
        }

        [HttpGet(Name = "GetAlunos")]
        public ActionResult<IEnumerable<Aluno>> GetAlunos()
        {
            List<Aluno> response = _alunosRepo.GetAll();
            return Ok(response);
        }

        [HttpPost]
        public ActionResult< Aluno > AddAluno([FromBody] AlunoDTO alunoDTO)
        {
            var aluno = new Aluno()
            {
                Matricula = alunoDTO.Matricula,
                Nome = alunoDTO.Nome
            };

            _alunosRepo.Insert(aluno);

            return Created("https://localhost:7146/api/FIAP", aluno);
        }
    }
}
