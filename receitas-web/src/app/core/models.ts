export interface ReceitaIngrediente {
  ingredienteId: number;
  ingrediente: string;
  quantidade: number;
  unidade: string;
}

export interface Receita {
  id: number;
  titulo: string;
  modoPreparacao: string;
  categoriaId: number;
  categoria: string;
  dificuldade: number;
  duracaoMinutos: number;
  validada: boolean;
  ingredientes: ReceitaIngrediente[];
}

export interface FiltroReceitas {
  texto?: string;
  categoriaId?: number;
  dificuldade?: number;
  duracaoMax?: number;
  ingrediente?: string;
}

export const DIFICULDADES: Record<number, string> = {
  1: 'Fácil',
  2: 'Média',
  3: 'Difícil',
};

export interface Login {
  email: string;
  password: string;
}

export interface Registo {
  nome: string;
  email: string;
  password: string;
}

export interface AuthResposta {
  token: string;
  expiraEm: string;
  nome: string;
  email: string;
  perfil: string;
}

export interface Categoria {
  id: number;
  nome: string;
}

export interface Ingrediente {
  id: number;
  nome: string;
  tipoMedida: number;
  unidades: string[];
}

export interface ReceitaIngredienteGuardar {
  ingredienteId: number;
  quantidade: number;
  unidade: string;
}

export interface ReceitaGuardar {
  titulo: string;
  modoPreparacao: string;
  categoriaId: number;
  dificuldade: number;
  duracaoMinutos: number;
  ingredientes: ReceitaIngredienteGuardar[];
}

export interface NovoIngrediente {
  nome: string;
  tipoMedida: number;
}

export interface IngredienteGestao {
  id: number;
  nome: string;
  validado: boolean;
  tipoMedida: number;
}

export const TIPOS_MEDIDA = [
  { valor: 1, texto: 'Unidade (ovos, alho...)' },
  { valor: 2, texto: 'Peso (g, kg)' },
  { valor: 3, texto: 'Volume (ml, l)' },
];