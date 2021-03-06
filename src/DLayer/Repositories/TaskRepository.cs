﻿using Core.Enum;
using Core.Interfaces;
using DLayer.Entities;
using DLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DLayer.Repositories
{
    public class TaskRepository : BaseRepository, ITaskRepository<Task>
    {
        private SqlConnection _connection;
        public TaskRepository() : base()
        {
            _connection = new SqlConnection();
        }
        public void Delete(int id)
        {
            string sp = "spDeleteTask";
            string taskId = "@TaskId";
            ExecuteNonQuery(sp, GetId(taskId,id));
        }

        public void Edit(Task entity)
        {
            string sp = "spUpdateTask";
            var parametersList = AddParameters(entity);
            ExecuteNonQuery(sp, parametersList);
            AddEmployeeTasks(entity);
        }

        public IEnumerable<Task> GetAllWithPaging(int pageNumber)
        {
            string sp = "spGetAllTasksPaging";
            var parametersList = GetParameters(pageNumber);
            return ExecuteReader<IList<Task>>(sp, parametersList, _connection, listsMapper);
        }

        public Task GetById(int id)
        {
            string sp = "spGetTaskById";
            string taskId = "@TaskId";
            return ExecuteReader<IList<Task>>(sp, GetId(taskId, id), _connection, listsMapper).First();
        }

        public int Insert(Task entity)
        {
            string sp = "spAddTask";
            var parametersList = AddParameters(entity);
            int taskId = ExecuteReader<int>(sp, parametersList, _connection, idMapper);
            entity.Id = taskId;
            AddEmployeeTasks(entity);
            return taskId;
        }

        private Func<SqlDataReader, IList<Task>> listsMapper = (sqlDataReader) =>
        {
            List<Task> taskList = new List<Task>();

            while (sqlDataReader.Read())
            {
                Task task = new Task();
                task.Id = Convert.ToInt32(sqlDataReader["Id"]);
                task.Name = sqlDataReader["Name"].ToString();
                task.TaskTime = Convert.ToInt64(sqlDataReader["TaskTime"]);
                task.DateOfStart = Convert.ToDateTime(sqlDataReader["DateOfStart"]);
                task.DateOfEnd = Convert.ToDateTime(sqlDataReader["DateOfEnd"]);
                task.TypeStatus = (Status)Enum.Parse(typeof(Status), sqlDataReader["TypeStatus"].ToString());
                task.ProjectId = Convert.ToInt32(sqlDataReader["ProjectId"]);
                taskList.Add(task);
            }

            return taskList;
        };

        public IEnumerable<Task> GetAll()
        {
            string sp = "spGetAllTasks";
            return ExecuteReader<IList<Task>>(sp, null, _connection, listsMapper);
        }

        private List<SqlParameter> AddParameters(Task entity)
        {
            List<SqlParameter> parametersList = new List<SqlParameter>
            {
                new SqlParameter("@Name", entity.Name),
                new SqlParameter("@TaskTime", entity.TaskTime),
                new SqlParameter("@DateOfStart", entity.DateOfStart),
                new SqlParameter("@DateOfEnd", entity.DateOfEnd),
                new SqlParameter("@Status", entity.TypeStatus),
                new SqlParameter("@ProjectId", entity.ProjectId)
            };

            if (entity.Id == 0)
                return parametersList;
            else
                parametersList.Add(new SqlParameter("@TaskId", entity.Id));
            return parametersList;
        }

        public IEnumerable<Task> GetTasksByProjectId(int id)
        {
            string sp = "spGetTasksByProjectId";
            string projectId = "@ProjectId";
            return ExecuteReader<IList<Task>>(sp, GetId(projectId, id), _connection, listsMapper);
        }

        private void AddEmployeeTasks(Task entity)
        {
            string storedProcedure = "spAddTasksStaff";

            foreach (var item in entity.EmployeeTasks)
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@StaffId", item.EmployeeId));
                parameters.Add(new SqlParameter("@TaskId", entity.Id));
                ExecuteNonQuery(storedProcedure, parameters);
            }

        }

        public IEnumerable<Task> GetTasksByEmployeeId(int id)
        {
            string sp = "spGetTasksByEmployeeId";
            string projectId = "@EmployeeId";
            return ExecuteReader<IList<Task>>(sp, GetId(projectId, id), _connection, listsMapper);
        }
    }
}
