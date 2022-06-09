-- Users (Services) table.
create table users
(
    id bigserial not null constraint users_pk primary key,
    user_name varchar(64) not null,
    api_key varchar(64) not null,
    callback varchar(64) not null,
    date_created timestamp with time zone DEFAULT now(),
);

-- States table.
create table states
(
    id bigserial not null constraint states_pk primary key,
    user_id bigint not null,
    state_name varchar(64) not null,
    state_code integer not null,
    date_created timestamp with time zone DEFAULT now()
    expiration_time number,

    CONSTRAINT fk_states_users
        FOREIGN KEY(user_id)
            REFERENCES users(id)
);

-- Actions table. Pre-defined in code.
create table actions
(
    id bigserial not null constraint actions_pk primary key,
    action_code integer not null,
);

-- Actions and States relation. Many-To-Many.
create table actions_states
(
    id bigserial not null constraint actions_pk primary key,
    action_id bigint not null,
    state_id bigint not null,
    
    CONSTRAINT fk_actions_states_action
        FOREIGN KEY(action_id)
            REFERENCES actions(id),
    CONSTRAINT fk_actions_states_states
        FOREIGN KEY(state_id)
            REFERENCES states(id)
);

-- Orders (transactions) table.
create table orders
(
    external_ref varchar(64) not null constraint orders_pk primary key,
    user_id bigint not null,
    amount numeric,
    source varchar(64),
    destination varchar(64),
    date_created timestamp with time zone DEFAULT now(),

    CONSTRAINT fk_orders_users
        FOREIGN KEY(user_id)
            REFERENCES users(id)
);

-- Events table. Pre-defined in code and in DB.
create table events
(
    id bigserial not null constraint events_pk primary key,
    event_code integer not null
);

-- Scenarios table.
create table scenarios
(
    id bigserial not null constraint scenarios_pk primary key,
    user_id bigint not null,
    rule json not null, 

    CONSTRAINT fk_scenarios_users
        FOREIGN KEY(user_id)
            REFERENCES users(id)
);

-- Events history table.
create table events_history
(
    id bigserial not null constraint events_history_pk primary key,
    order_external_ref varchar(64) not null,
    scenario_id bigint not null,
    state_id bigint not null,
    event_id bigint not null,

    CONSTRAINT fk_events_history_orders
        FOREIGN KEY(order_external_ref)
            REFERENCES orders(external_ref),
    CONSTRAINT fk_events_history_scenarios
        FOREIGN KEY(user_id)
            REFERENCES scenarios(id),
    CONSTRAINT fk_events_history_states
        FOREIGN KEY(state_id)
            REFERENCES states(id),
    CONSTRAINT fk_events_history_events
        FOREIGN KEY(event_id)
            REFERENCES events(id)
);

-- Seeds for inserting pre-defined events.
insert into events (event_code) (0); -- NumberOfOperationsAboveAverageEvent
insert into events (event_code) (1); -- MinimumOperationIntervalEvent
insert into events (event_code) (2); -- AmountOfOperationsAboveAverageEvent

-- Seed for inserting pre-defined actions
insert into actions (action_code) (0); -- TemporaryBlockAction
insert into actions (action_code) (1); -- BlockAction
insert into actions (action_code) (2); -- RequestVerification
