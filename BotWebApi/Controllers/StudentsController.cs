using AutoMapper;
using BotWebApi.Data;
using BotWebApi.Dtos;
using BotWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BotWebApi.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepo _repository;
        private readonly IMapper _mapper;



        //private readonly MockStudentRepo _repository = new MockStudentRepo();
        public StudentsController(IStudentRepo repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Student>> GetAllStudents()
        {
            var studentItems = _repository.GetAllStudents();
            return Ok(_mapper.Map<IEnumerable<StudentReadDto>>(studentItems));
        }
        [HttpGet("{id}")]
        public ActionResult<StudentReadDto>GetStudentById(int id)
        {
            var studentItem = _repository.GetStudentById(id);
            if (studentItem != null)
            {
                return Ok(_mapper.Map<StudentReadDto>(studentItem));
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<string> CreateStudent(StudentCreateDto studentCreateDto)
        {
            var studentModel = _mapper.Map<Student>(studentCreateDto);
            _repository.CreateStudent(studentModel);
            var studentModel1 = _mapper.Map<StudentReadDto>(studentModel);
            HttpClient client = new HttpClient();
            //ADD your ngrok url
            client.BaseAddress = new Uri("https://3a5e06b6ad12.ngrok.io");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //GET Method  
            var body = JsonConvert.SerializeObject(studentModel1, Formatting.Indented, new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.All
            });
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            HttpResponseMessage response =  await client.PostAsync("api/notify", content);

            if (response.IsSuccessStatusCode)
            {
                // Get the URI of the created resource.  
                Uri returnUrl = response.Headers.Location;
                Console.WriteLine(returnUrl);
            }
            var studentReadDto = _mapper.Map<StudentReadDto>(studentModel);
            return "success";
        }
        
    }
}
