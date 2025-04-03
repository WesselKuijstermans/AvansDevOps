// See https://aka.ms/new-console-template for more information

using AvansDevOps.Adapter;
using AvansDevOps.FactoryPattern;

ITeamMemberFactory factory = new DeveloperFactory();
var developer = factory.CreateTeamMember("John Doe", new EmailAdapter());

developer.notify("Hello World!");