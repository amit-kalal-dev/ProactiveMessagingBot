using AutoMapper;
using BotWebApi.Dtos;
using BotWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotWebApi.Profiles
{
    public class StudentsProfile : Profile
    {
        public StudentsProfile()
        {
            CreateMap<Student, StudentReadDto>();
            CreateMap<StudentReadDto,Student>();
            CreateMap<StudentCreateDto, Student>();
        }
    }
}
