﻿using Microsoft.AspNetCore.Mvc;
using PayCal.Models;
using PayCal.Repositories;
using PayCal.Logging;
using log4net;
using System.Reflection;

namespace PayCal_API.Controllers
{
    [ApiController]
    [Route("~/Temporary-Employees")]
    public class TempEmployeeController : Controller
    {
        private readonly ILog _log;
        private readonly IRepository<TempEmployeeData> _temp;

        public TempEmployeeController(IRepository<TempEmployeeData> temp)
        {
            _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            _temp = temp;
        }

        [HttpGet()]
        public IActionResult GetTempEmployees()
        {
            var response = _temp.ReadAll();
            if (response == null) {
                _log.Warn($"\nGET: {LogStrings.defaultmsg} {LogStrings.http204}\n{LogStrings.context204}");
                return NoContent();
            }
            else {
                _log.Info($"\nGET: {LogStrings.defaultmsg} {LogStrings.http200}");
                return Ok(response);
            }
        }

        [HttpGet("{ID}")]
        public IActionResult GetTempEmployeeByID(int ID)
        {
            var read = _temp.Read(ID);
            if (read != null) {
                _log.Warn($"\nGET: {LogStrings.defaultmsg} {LogStrings.http200}");
                return Ok(read);
            }
            else {
                _log.Info($"\nGET: {LogStrings.defaultmsg} {LogStrings.http404}\n{LogStrings.context404}");
                return NotFound();
            }
        }

        [HttpPut("{ID}")]
        public IActionResult UpdateTempEmployee(int ID, string fname, string lname, int? dayrate, int? weeksworked)
        {
            var response =_temp.Update(ID, fname, lname, dayrate, weeksworked);
            if (response == null) {
                _log.Warn($"\nPUT: {LogStrings.defaultmsg} {LogStrings.http404}\n{LogStrings.context404}");
                return NotFound();
            }
            else {
                _log.Info($"\nPUT: {LogStrings.defaultmsg} {LogStrings.http204}\n{LogStrings.context204}");
                return NoContent();
            }
        }

        [HttpPost()]
        public IActionResult PostNewTempEmployee(string fname, string lname, int dayrate, int weeksworked)
        {
            var response = _temp.Create(fname, lname, dayrate, weeksworked);
            string uri = ($"{response.EmployeeID}");
            _log.Info($"\nPOST: {LogStrings.defaultmsg} {LogStrings.http201}\n{LogStrings.context201}");
            return Created(uri, response);
        }

        [HttpDelete("{ID}")]
        public IActionResult DeleteTempEmployee(int ID)
        {
            var delete = _temp.Delete(ID);
            if (delete) {
                _log.Info($"\nDELETE: {LogStrings.defaultmsg} {LogStrings.http200}");
                return Ok(delete);
            }
            else {
                _log.Error($"\nDELETE: {LogStrings.errormsg}\n{LogStrings.defaultmsg} {LogStrings.http400}\n{LogStrings.context400}");
                return BadRequest();
            }
        }
    }
}