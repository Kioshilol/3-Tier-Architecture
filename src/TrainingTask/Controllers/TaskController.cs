﻿using System.Collections.Generic;
using BLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrainingTask.Mapper;
using TrainingTask.Mapping;
using TrainingTask.Models;

namespace TrainingTask.Controllers
{
    public class TaskController : Controller
    {
        private TaskService taskService;
        private TaskMapper taskMapper;
        private ProjectMapper projectMapper;
        private ProjectService projectService;
        public TaskController()
        {
            this.taskService = new TaskService();
            this.taskMapper = new TaskMapper();
            this.projectService = new ProjectService();
            this.projectMapper = new ProjectMapper();
        }
        public IActionResult Task()
        {
            var taskViewModelList = new List<TaskViewModel>();
            var taskList = taskService.GetAll();
            foreach (var task in taskList)
            {
                var taskViewModel = taskMapper.Map(task);
                taskViewModelList.Add(taskViewModel);
            }
            return View(taskViewModelList);
        }

        public IActionResult CreateTask()
        {
            var projectsDTO = projectService.GetAll();
            var projectsViewModel = new List<ProjectViewModel>();
            foreach(var project in projectsDTO)
            {
                var projectViewModel = projectMapper.Map(project);
                projectsViewModel.Add(projectViewModel);
                ViewBag.Projects = projectsViewModel;
            }
            return View();
        }

        [HttpPost]
        public IActionResult CreateTask(TaskViewModel task)
        {
            var taskDTO = taskMapper.Map(task);
            taskService.Add(taskDTO);
            return RedirectToAction("Task");
        }

        public IActionResult EditTask(int id)
        {
            var taskDTO = taskService.GetById(id);
            var taskModelView = taskMapper.Map(taskDTO);
            return View(taskModelView);
        }

        [HttpPost]
        public IActionResult EditTask(TaskViewModel task)
        {
            var taskDTO = taskMapper.Map(task);
            taskService.Edit(taskDTO);
            return RedirectToAction("Task");
        }

        public IActionResult DeleteTask(int id)
        {
            var task = taskService.GetById(id);
            var taskViewModel = taskMapper.Map(task);
            return View(taskViewModel);
        }

        [HttpPost, ActionName("DeleteTask")]
        public IActionResult DeleteConfirmed(int id)
        {
            taskService.Delete(id);
            return RedirectToAction("Task");
        }

        public IActionResult ShowTask(int id)
        {
            var taskDTO = taskService.GetById(id);
            var taskModelView = taskMapper.Map(taskDTO);
            return View(taskModelView);
        }
    }
}