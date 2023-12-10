use std::ops::{Add, Sub};

#[derive(Debug, Clone, Copy, PartialEq, Eq)]
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
    pub fn is_connected_to_south(&self) -> bool {
        match self {
            Pipe::NorthAndSouth => true,
            Pipe::SouthAndEast => true,
            Pipe::SouthAndWest => true,
            _ => false,
        }
    }

    pub fn is_connected_to_north(&self) -> bool {
        match self {
            Pipe::NorthAndSouth => true,
            Pipe::NorthAndEast => true,
            Pipe::NorthAndWest => true,
            _ => false,
        }
    }

    pub fn is_connected_to_east(&self) -> bool {
        match self {
            Pipe::EastAndWest => true,
            Pipe::NorthAndEast => true,
            Pipe::SouthAndEast => true,
            _ => false,
        }
    }

    pub fn is_connected_to_west(&self) -> bool {
        match self {
            Pipe::EastAndWest => true,
            Pipe::NorthAndWest => true,
            Pipe::SouthAndWest => true,
            _ => false,
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
    pub fn new(x: i32, y: i32) -> Self {
        Self { x, y }
    }

    pub fn up() -> Self {
        Self::new(0, -1)
    }

    pub fn down() -> Self {
        Self::new(0, 1)
    }

    pub fn left() -> Self {
        Self::new(-1, 0)
    }

    pub fn right() -> Self {
        Self::new(1, 0)
    }

    pub fn up_left() -> Self {
        Self::new(-1, -1)
    }

    pub fn up_right() -> Self {
        Self::new(1, -1)
    }

    pub fn down_left() -> Self {
        Self::new(-1, 1)
    }

    pub fn down_right() -> Self {
        Self::new(1, 1)
    }

    pub fn is_adjacent(&self, other: &Self) -> bool {
        let diff = *self - *other;
        diff.x.abs() <= 1 && diff.y.abs() <= 1
    }

    pub fn get_adjacent(&self, include_diagonals: bool) -> Vec<Self> {
        let mut result = Vec::new();
        result.push(*self + Self::up());
        result.push(*self + Self::down());
        result.push(*self + Self::left());
        result.push(*self + Self::right());
        if include_diagonals {
            result.push(*self + Self::up_left());
            result.push(*self + Self::up_right());
            result.push(*self + Self::down_left());
            result.push(*self + Self::down_right());
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
