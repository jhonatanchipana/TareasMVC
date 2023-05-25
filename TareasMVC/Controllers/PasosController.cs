using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using TareasMVC.Entidades;
using TareasMVC.Migrations;
using TareasMVC.Models;
using TareasMVC.Servicios;
using TareasMVC.Servicios.Interface;

namespace TareasMVC.Controllers
{
    [Route("api/pasos")]
    public class PasosController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly IServicioUsuarios _servicioUsuario;

        public PasosController(ApplicationContext context, 
            IServicioUsuarios servicioUsuario)
        {
            _context = context;
            _servicioUsuario = servicioUsuario;
        }

        [HttpPost("{tareaId:int}")]
        public async Task<ActionResult<Paso>> Post(int tareaId, [FromBody] PasosCrearDTO pasosCrearDTO)
        {
            var usuarioId = _servicioUsuario.ObtenerUsuarioId();
            var tarea = await _context.Tareas.FirstOrDefaultAsync(t => t.Id == tareaId);

            if (tarea is null)
            {
                //retornas nulo
                return NotFound();
            }

            if(tarea.UsuarioCreacionId != usuarioId)
            {
                //no tiene acceso, prohibido
                return Forbid();
            }

            var existenPasos = await _context.Pasos.AnyAsync(p => p.TareaId == tareaId);

            var ordenMayor = 0;
            if (existenPasos)
            {
                ordenMayor = await _context.Pasos
                        .Where(p => p.TareaId == tareaId)
                        .Select(p => p.Orden).MaxAsync();
            }

            var paso = new Paso();
            paso.TareaId = tareaId;
            paso.Orden = ordenMayor + 1;
            paso.Descripcion = pasosCrearDTO.Descripcion;
            paso.Realizado = pasosCrearDTO.Realizado;

            _context.Add(paso);
            await _context.SaveChangesAsync();

            return paso;

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody] PasosCrearDTO pasosCrearDTO)
        {
            var usuarioId = _servicioUsuario.ObtenerUsuarioId();
            var paso = await _context.Pasos.Include(p => p.Tarea).FirstOrDefaultAsync(p => p.Id == id);

            if (paso is null)
            {
                return NotFound();
            }

            if (paso.Tarea.UsuarioCreacionId != usuarioId)
            {
                return Forbid();
            }

            paso.Descripcion = pasosCrearDTO.Descripcion;
            paso.Realizado = pasosCrearDTO.Realizado;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var usuarioId = _servicioUsuario.ObtenerUsuarioId();

            var paso = await _context.Pasos.Include(p => p.Tarea).FirstOrDefaultAsync(t => t.Id == id);

            if (paso is null)
            {
                return NotFound();
            }

            if (paso.Tarea.UsuarioCreacionId != usuarioId)
            {
                return Forbid();
            }

            _context.Pasos.Remove(paso);
            await _context.SaveChangesAsync();
            
            return Ok();
        }

        [HttpPost("ordenar/{tareaId:int}")]
        public async Task<IActionResult> Ordenar(int tareaId, [FromBody] Guid[] ids)
        {
            var usuarioId = _servicioUsuario.ObtenerUsuarioId();

            var tarea = await _context.Tareas.FirstOrDefaultAsync(t => t.Id == tareaId && t.UsuarioCreacionId == usuarioId);

            if(tarea is null)
            {
                return NotFound();
            }

            var pasos = await _context.Pasos.Where(t => t.TareaId == tareaId).ToListAsync();

            var pasosIds = pasos.Select(x => x.Id);

            var idsPasosNoPertenecenALaTare = ids.Except(pasosIds).ToList();

            if (idsPasosNoPertenecenALaTare.Any())
            {
                return BadRequest("No todos los pasos están presentes");
            }

            var pasosDiccionario = pasos.ToDictionary(p => p.Id);

            for (int i = 0; i < ids.Length; i++)
            {
                var pasoId = ids[i];
                var paso = pasosDiccionario[pasoId];

                paso.Orden = i + 1;
            }

            await _context.SaveChangesAsync();

            return Ok();

        }
    }
}
