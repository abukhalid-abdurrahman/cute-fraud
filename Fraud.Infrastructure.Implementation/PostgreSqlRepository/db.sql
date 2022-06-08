-- Users (Services) table.
create table users
(
    id bigserial not null constraint users_pk primary key,
    user_name varchar(64) not null,
    api_key varchar(64) not null,
    callback varchar(64) not null,
    date_created timestamp with time zone,
);

-- States table.
create table states
(
    id bigserial not null constraint states_pk primary key,
    user_id bigint not null,
    state_name varchar(64) not null,
    state_type integer not null,
    expiration_time timestamp with time zone,

    CONSTRAINT fk_states_users
        FOREIGN KEY(user_id)
            REFERENCES users(id)
);

-- Actions table. Pre-defined in code.
create table actions
(
    id bigserial not null constraint actions_pk primary key,
    state_id bigint not null,
    action_name varchar(64) not null,

    CONSTRAINT fk_actions_states
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
    date_created timestamp with time zone,

    CONSTRAINT fk_orders_users
        FOREIGN KEY(user_id)
            REFERENCES users(id)
);

-- Events table. Pre-defined in code and in DB.
create table events
(
    id bigserial not null constraint events_pk primary key,
    event_name varchar(64) not null
);

-- Scenarios table.
create table scenarios
(
    id bigserial not null constraint scenarios_pk primary key,
    user_id bigint not null,
    scenario json not null, 

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

create table payments_history
(
    id bigserial not null constraint payments_history_pk primary key,
    payment_history_request_id bigint not null,
    gateway_order_id varchar(64),
    payment_reference varchar(64),
    terminal_id varchar(64),
    merchant_id varchar(64),
    ref_number varchar(64),
    stan varchar(64),
    pan varchar(64),
    amount numeric,
    status varchar(64),
    merchant_name varchar(64),
    date_created timestamp with time zone,
    CONSTRAINT fk_payment_history_request
        FOREIGN KEY(payment_history_request_id)
            REFERENCES payments_history_requests(id)
);