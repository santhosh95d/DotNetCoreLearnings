using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWEBAPI.Data;
using MyWEBAPI.DTOs;
using MyWEBAPI.Models;

namespace MyWEBAPI.Controllers
{
    [Route("api/[controller]")] //api/Commands
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private ICommanderRepo _repository;
        private IMapper _mapper;

        //private readonly MockCommanderRepo _repository = new MockCommanderRepo();
        public CommandsController(ICommanderRepo repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Command>> GetAllCommands() //actionresult<t> allows us to return object of specific tyoe and also can return status codes
        {
            var commandItems=_repository.GetAllCommands();

            return Ok(_mapper.Map<IEnumerable<CommandReadDTO>>(commandItems));

        }

        [HttpGet("{id}",Name ="GetCommandById")]
        public ActionResult<CommandReadDTO> GetCommandById(int id)
        {
            var CommandItem = _repository.GetCommandById(id);
            if (CommandItem != null)
            {
                return Ok(_mapper.Map<CommandReadDTO>(CommandItem));
            }
            return NotFound();
        }

        [HttpPost]
        public ActionResult<CommandReadDTO> CreateCommand(CommandCreateDTO commandCreateDto)
        {
            var commandmodel = _mapper.Map<Command>(commandCreateDto);
            _repository.CreateCommand(commandmodel);

            _repository.SaveChanges();

            var commandreadDTO=_mapper.Map<CommandReadDTO>(commandmodel);

            //return Ok(commandCreateDto);
            return CreatedAtRoute(nameof(GetCommandById), new { Id = commandreadDTO.Id }, commandreadDTO); //RETURNS 201 CREATED WITH URI
        }

        [HttpPut("{id}")]

        public ActionResult UpdateCommand(int id,CommandUpdateDto commandUpdateDto)
        {
            var commandModelfromRepo = _repository.GetCommandById(id);
            if (commandModelfromRepo == null)
            {
                return NotFound();
            }
            _mapper.Map(commandUpdateDto, commandModelfromRepo);//since we have updated the commandmodel here dbcontext keeps track of the changes here and hence calling save changes will do the trick.

            _repository.UpdateCommand(commandModelfromRepo);//just the dummy implementation

            _repository.SaveChanges();

            return NoContent();
        }
        [HttpDelete("{id}")]

        public ActionResult DeleteCommand(int id)
        {
            var commandModelRepo = _repository.GetCommandById(id);
            if (commandModelRepo==null)
            {
                return NotFound();
            }
            _repository.DeleteCommand(commandModelRepo);
            _repository.SaveChanges();
            return NoContent();
        }







    }
}
