use std::ops::{Add, Sub};

#[derive(Debug, Clone, Copy, PartialEq, Eq, Hash)]
pub enum Pipe {
    Start,
    NorthAndSouth,
    EastAndWest,
    NorthAndEast,
    NorthAndWest,
    SouthAndWest,
    SouthAndEast,
    Ground,
}

impl From<char> for Pipe {
    fn from(value: char) -> Self {
        match value {
            'S' => Pipe::Start,
            '|' => Pipe::NorthAndSouth,
            '-' => Pipe::EastAndWest,
            'L' => Pipe::NorthAndEast,
            'J' => Pipe::NorthAndWest,
            '7' => Pipe::SouthAndWest,
            'F' => Pipe::SouthAndEast,
            '.' => Pipe::Ground,
            _ => panic!("Unknown pipe: {}", value),
        }
    }
}

impl Pipe {
    pub fn has_south_connection(&self) -> bool {
        match self {
            Pipe::NorthAndSouth => true,
            Pipe::SouthAndEast => true,
            Pipe::SouthAndWest => true,
            _ => false,
        }
    }

    pub fn has_north_connection(&self) -> bool {
        match self {
            Pipe::NorthAndSouth => true,
            Pipe::NorthAndEast => true,
            Pipe::NorthAndWest => true,
            _ => false,
        }
    }

    pub fn has_east_connection(&self) -> bool {
        match self {
            Pipe::EastAndWest => true,
            Pipe::NorthAndEast => true,
            Pipe::SouthAndEast => true,
            _ => false,
        }
    }

    pub fn has_west_connection(&self) -> bool {
        match self {
            Pipe::EastAndWest => true,
            Pipe::NorthAndWest => true,
            Pipe::SouthAndWest => true,
            _ => false,
        }
    }
}

impl Pipe {
    pub fn get_next_directions(&self) -> Vec<Point> {
        match self {
            Pipe::Start => vec![Point::UP, Point::DOWN, Point::LEFT, Point::RIGHT],
            Pipe::NorthAndSouth => vec![Point::UP, Point::DOWN],
            Pipe::EastAndWest => vec![Point::LEFT, Point::RIGHT],
            Pipe::NorthAndEast => vec![Point::UP, Point::RIGHT],
            Pipe::NorthAndWest => vec![Point::UP, Point::LEFT],
            Pipe::SouthAndWest => vec![Point::DOWN, Point::LEFT],
            Pipe::SouthAndEast => vec![Point::DOWN, Point::RIGHT],
            _ => panic!("Shouldn't be looking for next point"),
        }
    }
}

pub fn parse_input(input: &str) -> Vec<Vec<Pipe>> {
    input
        .lines()
        .map(|line| line.chars().map(|pipe| pipe.into()).collect::<Vec<Pipe>>())
        .collect()
}

#[derive(Debug, Clone, Copy, PartialEq, Eq, Hash)]
pub struct Point {
    pub x: i32,
    pub y: i32,
}

impl Point {
    pub const UP: Point = Point { x: 0, y: -1 };
    pub const DOWN: Point = Point { x: 0, y: 1 };
    pub const LEFT: Point = Point { x: -1, y: 0 };
    pub const RIGHT: Point = Point { x: 1, y: 0 };
    pub const UP_LEFT: Point = Point { x: -1, y: -1 };
    pub const UP_RIGHT: Point = Point { x: 1, y: -1 };
    pub const DOWN_LEFT: Point = Point { x: -1, y: 1 };
    pub const DOWN_RIGHT: Point = Point { x: 1, y: 1 };

    pub fn new(x: i32, y: i32) -> Self {
        Self { x, y }
    }
    pub fn is_adjacent(&self, other: &Self) -> bool {
        let diff = *self - *other;
        diff.x.abs() <= 1 && diff.y.abs() <= 1
    }

    pub fn get_adjacent(&self, include_diagonals: bool) -> Vec<Self> {
        let mut result = Vec::new();
        result.push(*self + Self::UP);
        result.push(*self + Self::DOWN);
        result.push(*self + Self::LEFT);
        result.push(*self + Self::RIGHT);
        if include_diagonals {
            result.push(*self + Self::UP_LEFT);
            result.push(*self + Self::UP_RIGHT);
            result.push(*self + Self::DOWN_LEFT);
            result.push(*self + Self::DOWN_RIGHT);
        }
        result
    }
}

impl Add for Point {
    type Output = Self;

    fn add(self, other: Self) -> Self {
        Self::new(self.x + other.x, self.y + other.y)
    }
}

impl Sub for Point {
    type Output = Self;

    fn sub(self, other: Self) -> Self {
        Self::new(self.x - other.x, self.y - other.y)
    }
}
