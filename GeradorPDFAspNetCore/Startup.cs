using DinkToPdf;
using DinkToPdf.Contracts;
using GeradorPDFAspNetCore.Services;
using jsreport.AspNetCore;
using jsreport.Binary;
using jsreport.Local;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Rotativa.AspNetCore;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace GeradorPDFAspNetCore
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public Startup(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Configuração do JS Report para gerar o relatório em PDF
            services.AddJsReport(new LocalReporting()
                    .UseBinary(JsReportBinary.GetBinary())
                    .AsUtility()
                    .Create());

            // Configurações do DinkToPDF
            // Verifica qual a arquiterura para utilizar os arquivos necessários
            var architectureFolder = (IntPtr.Size == 8) ? "64 bit" : "32 bit";
            // Encontra o caminho onde estão os arquivos
            var wkHtmlToPdfPath = Path.Combine(_hostingEnvironment.ContentRootPath, $"v0.12.4\\{architectureFolder}\\libwkhtmltox");
            // Carrega os arquivos necessários, passadas as configurações
            CustomAssemblyLoadContext context = new CustomAssemblyLoadContext();
            context.LoadUnmanagedLibrary(wkHtmlToPdfPath);

            // Configuração do DinkToPdf
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            // Configuração do conversor de razor views para string
            services.AddScoped<IViewRenderService, ViewRenderService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseMvc();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // Configuração do rotativa
            // Isso serve para que o rotativa utilize os arquivos presentes na pasta wwwroot/Rotativa
            RotativaConfiguration.Setup(env);
        }
    }

    // Classe resposnsável por carregar os arquivos necessários para o DinkToPDF
    internal class CustomAssemblyLoadContext : AssemblyLoadContext
    {
        public IntPtr LoadUnmanagedLibrary(string absolutePath)
        {
            return LoadUnmanagedDll(absolutePath);
        }

        protected override IntPtr LoadUnmanagedDll(String unmanagedDllName)
        {
            return LoadUnmanagedDllFromPath(unmanagedDllName);
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            throw new NotImplementedException();
        }
    }
}
