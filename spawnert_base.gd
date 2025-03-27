extends RigidBody2D

@export var spawn_object  = preload("res://Resourses/Ores and resourses/ore_base.tscn")
#<-----------------------------Properties of entity---------------------------->

@export var spawner_hp		:int		= 20

@export var max_spawner_hp	:int		= 20

@export var click_damage	:int 		= 5;

@export var max_force_x		:int		= 100

@export var min_force_x		:int		= 60

@export var max_force_y		:int		= -350 #Negative to make it go up

@export var min_force_y		:int		= -275 

@export var velocity		:Vector2	= Vector2.ZERO

var MinObjectsToSpawn		:int 		= 1;

var MaxObjectsToSpawn		:int 		= 6;

var force_x					:float		= 0.0

var force_y					:float		= 0.0

var gravity_scale_custom	:float		= 0.05



func _ready():
	pass


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass



func _on_click_zone_input_event(viewport, event, shape_idx):
	if  event is InputEventMouseButton and event.pressed and event.button_index == MOUSE_BUTTON_LEFT:
		if spawner_hp <= 0:
			var spawnNumber:int = randf_range(MinObjectsToSpawn, MaxObjectsToSpawn)
			for n in spawnNumber:
				var obj = spawn_object.instantiate()
				add_sibling(obj)
				obj.position = Vector2(self.position.x+35,self.position.y-35)
				obj.lock_rotation = true
				spawner_hp = max_spawner_hp
		else:
			spawner_hp -= click_damage
