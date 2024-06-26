﻿using ToDoApp.Model;

namespace ToDoApp.ViewModel;

public sealed class Context
{
    public ViewContextSide ViewSide { get; }
    
    public ModelContextSide ModelSide { get; }
    
    public Context(Settings initSettings, IEnumerable<ToDoTask>? initTasks)
    {
        ArgumentNullException.ThrowIfNull(initSettings, nameof(initSettings));

        var collectionChanger = new TasksCollectionChanger();
        var tasksCollection = new TasksCollection(initTasks ?? Enumerable.Empty<ToDoTask>(), 
            collectionChanger);

        ViewSide = new ViewContextSide(initSettings, tasksCollection, collectionChanger);
        
        ModelSide = new ModelContextSide(initSettings, tasksCollection);
        ModelSide.BindViewSide(ViewSide, collectionChanger);
    }
}