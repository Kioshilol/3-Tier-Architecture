﻿using AutoMapper;
using BLayer.DTO;
using DLayer.Entities;

namespace BLayer.Mappers.AutoMappers
{
    public class AutoMapperConfiguration
    {
        protected  MapperConfiguration GetConfiguration()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<EmployeeDTO, Employee>();
                cfg.CreateMap<Employee, EmployeeDTO>();
                cfg.CreateMap<ProjectDTO, Project>();
                cfg.CreateMap<Project, ProjectDTO>();
                cfg.CreateMap<EmployeeTasksDTO, EmployeeTasks>();
                cfg.CreateMap<EmployeeTasks, EmployeeTasksDTO>();
                cfg.CreateMap<TaskDTO, Task>().ForMember(dest => dest.EmployeeTasks, act => act.MapFrom(src => src.EmployeeTasks));
                cfg.CreateMap<Task, TaskDTO>().ForMember(dest => dest.EmployeeTasks, act => act.MapFrom(src => src.EmployeeTasks)); ;
            });
            return config;
        }
    }
}
